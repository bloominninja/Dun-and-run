using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class Node
{	
	public Node next = null;//our neighbor, used for iterating
	public NodeLink links = null;//the links that connect us to other nodes
	//if links is null, we assume we are the final node
	
	public double value = 0;//stores our sum
	//used for the frontmost nodes to get game data
	//used at the end as the calculated values

	public Node()
	{
		
	}
	
	public void calculate()//calculate until we are that very end with no nodes
	{
		//calculate our data
		//run through all links
		NodeLink linkIterator = links;
		
		//add the dot product for each one
		while(linkIterator!=null)
		{
			linkIterator.calculate();
			
			linkIterator = linkIterator.next;
		}
	}
}

class NodeLink
{
	double weight = 1.0;
	Node destination = null;
	Node origin = null;
	public NodeLink next = null;//used for iterating across links
	
	public NodeLink( Node orig, Node dest, double nweight)
	{
		destination = dest;
		origin = orig;
		weight = nweight;
	}
	
	public void calculate()//calculate until we are that very end with no nodes
	{
		//multiply output on origin by the weight and add to value at destination
		destination.value += weight * origin.value;
	}
	
	public void clear()
	{
		destination = null;
		origin = null;
	}
}

public class AiBase : MonoBehaviour
{
    public PlayerController ourPlayer;
	
		//Our neural network stuff is held here
	Node nodeLayerInput = null;
		//We have our position on the map
		//Our character
		//Our active items
		//Our passives
		//Our cooldowns
		//Our health, max health
		//Our mana, max mana
		//Time since last jump (jump cooldown)
		//Other player data
			//Position (x, y)
			//Distance from us (dx, dy)
			//health, max health
			//mana, max mana
			//Character (int for character id with gaps of 1000, no player is -1000)
			//Attack/item cooldowns
			//Active items (int for each item id * 100)
			//Passive items (int for each item id * 100, -1 for no passive here)
			//Time since last jump (jump cooldown)
	int inputLayerCount = 0;
	Node nodeLayer1 = null;//20 of these
	int layer1Count = 0;
	Node nodeLayer2 = null;//20 of these
	int layer2Count = 0;
	Node nodeLayerOutput = null;
	int outputLayerCount = 0;
	
	int directionCount = 0;
	
	public AiTrainerManager aiBroodmother = null;
		
	public double score = 0;//this is our performance with this genetic layout, higher score = better
		//modified during gameplay at real-time
	
	//scoring data
		//use this data for calculating the score, such as trickshots
		//may be useful for stats screen?
	
	protected double aiLastDirection = 0;

    void Start ()
    {
    }
	
    void Update ()
    {
		if(ourPlayer!=null)
		{
			//get game status info
			pollInputs();
			
			calculateOutputs();//run data through the machine
			
			//use the outputs of the network
			readOutputs();
			
			//check if the ai is going the same direction as before
			if(aiLastDirection == ourPlayer.aiDirection)
			{
				//if so, penalize it proportional to the time spent in that direction
				directionCount++;
				
				score -= directionCount / 100;
			}
			else
			{
				score+= 1;//small reward for mixing it out
				directionCount = 0;
			}
			
			//do not touch it if we aren't able to do anything
			//proc movement to the left just because
			if(ourPlayer.aiIdle)
				aiLastDirection = 0;
			else
				aiLastDirection = ourPlayer.aiDirection;
			
			//penalize straying from center of stage
			double distance = Math.Pow(ourPlayer.transform.position.x - 18,2);
			score -= distance/500;
		}
	}
	
	
	//this function will create our neural network, must pass in ALL weights needed here
	//all weights are from name->nextLayer
	public void createNetwork(double[] inputWeights, double[] layer1Weights, double[] layer2Weights)
	{
		//destroy our network before we can create a new network
		destroyNetwork();
		
		//start with creating nodes for each layer
		//input layer has 102 inputs between the four characters
		for(int i=0; i<inputLayerCount; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayerInput;
			nodeLayerInput = newNode;
		}
		//layer 1
		for(int i=0; i<layer1Count; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayer1;
			nodeLayer1 = newNode;
		}
		//layer 2
		for(int i=0; i<layer2Count; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayer2;
			nodeLayer2 = newNode;
		}
		//we have 9 outputs we need to worry about
		for(int i=0; i<outputLayerCount; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayerOutput;
			nodeLayerOutput = newNode;
		}
		
		//now create the links for it
		//four sets of weights
		//we will assume that we can continually keep pulling numbers without fear of going out of bounds until we have what we need
		//every node needs a link to every node in the next layer
		var iteratorLeft = nodeLayerInput;
		int index = 0;//this is our index in the weight array
		while(iteratorLeft!=null)
		{
			//iterate across all of the next layer
			var iteratorRight = nodeLayer1;
			while(iteratorRight != null)
			{
				//create link, push it onto the front
				var newLink = new NodeLink(iteratorLeft, iteratorRight, inputWeights[index]);
				newLink.next = iteratorLeft.links;
				iteratorLeft.links = newLink;
				
				iteratorRight = iteratorRight.next;
				index++;//up our index yos
			}
			
			iteratorLeft = iteratorLeft.next;
		}
		
		iteratorLeft = nodeLayer1;
		index = 0;//this is our index in the weight array
		while(iteratorLeft!=null)
		{
			//iterate across all of the next layer
			var iteratorRight = nodeLayer2;
			while(iteratorRight != null)
			{
				//create link, push it onto the front
				var newLink = new NodeLink(iteratorLeft, iteratorRight, layer1Weights[index]);
				newLink.next = iteratorLeft.links;
				iteratorLeft.links = newLink;
				
				iteratorRight = iteratorRight.next;
				index++;//up our index yos
			}
			
			iteratorLeft = iteratorLeft.next;
		}
		
		iteratorLeft = nodeLayer2;
		index = 0;//this is our index in the weight array
		while(iteratorLeft!=null)
		{
			//iterate across all of the next layer
			var iteratorRight = nodeLayerOutput;
			while(iteratorRight != null)
			{
				//create link, push it onto the front
				var newLink = new NodeLink(iteratorLeft, iteratorRight, layer2Weights[index]);
				newLink.next = iteratorLeft.links;
				iteratorLeft.links = newLink;
				
				iteratorRight = iteratorRight.next;
				index++;//up our index yos
			}
			
			iteratorLeft = iteratorLeft.next;
		}
	}
	
	void destroyNetwork()
	{
		if(nodeLayerInput == null || nodeLayer1 == null | nodeLayer2 == null | nodeLayerOutput == null)
		{
			return;//can't destroy what doesn't exist!
		}
		
		//reset our score
		score = 0;
		
		//delete our nodes
		while(nodeLayerInput != null)
		{
			//set each of our links to null
			while(nodeLayerInput.links != null)
			{
				nodeLayerInput.links.clear();
				
				nodeLayerInput.links = nodeLayerInput.links.next;
			}
			
			nodeLayerInput = nodeLayerInput.next;
		}
		
		while(nodeLayer1 != null)
		{
			//set each of our links to null
			while(nodeLayer1.links != null)
			{
				nodeLayer1.links.clear();
				
				nodeLayer1.links = nodeLayer1.links.next;
			}
			
			nodeLayer1 = nodeLayer1.next;
		}
		
		while(nodeLayer2 != null)
		{
			//set each of our links to null
			while(nodeLayer2.links != null)
			{
				nodeLayer2.links.clear();
				
				nodeLayer2.links = nodeLayer2.links.next;
			}
			
			nodeLayer2 = nodeLayer2.next;
		}
		
		while(nodeLayerOutput != null)
		{			
			nodeLayerOutput = nodeLayerOutput.next;
		}
	}
	
	void resetCalculations()
	{
		//reset calculations across all of the nodes
		var iterator = nodeLayerInput;
		
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer1;
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer2;
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayerOutput;
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
	}
	
	//this runs through our network to calculate individual outputs based on weights and the inputs
	void calculateOutputs()
	{
		resetCalculations();//need to reset all of the calculations before we make new ones
		
		
		//reset calculations across all of the nodes
		var iterator = nodeLayerInput;
		while(iterator != null)
		{			
			//calculate for each node
			iterator.calculate();
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer1;
		while(iterator != null)
		{			
			//make sure to squash our value
			iterator.value = squashFunction(iterator.value);
	
			//calculate for each node
			iterator.calculate();
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer2;
		while(iterator != null)
		{			
			//make sure to squash our value
			iterator.value = squashFunction(iterator.value);
	
			//calculate for each node
			iterator.calculate();
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayerOutput;
		while(iterator != null)
		{			
			//make sure to squash our value
			iterator.value = squashFunction(iterator.value);
	
			iterator = iterator.next;
		}
	}
	
	void pollInputs()
	{
		if(ourPlayer == null)
			return;//don't run without a player
		
		Node currNode = null;
		//Time since last jump (jump cooldown)
		//Other player data
			//Distance from us (dx, dy)
			
		//later (low priority)
			//passive ids
		
		//get our x and y
		currNode = getNode(nodeLayerInput, 0);
		if(currNode != null)
			currNode.value = ourPlayer.transform.position.x;
		
		currNode = getNode(nodeLayerInput, 1);
		if(currNode != null)
			currNode.value = ourPlayer.transform.position.y;
		
		currNode = getNode(nodeLayerInput, 2);
		if(currNode != null)
			currNode.value = ourPlayer.currentHealth;
		
		currNode = getNode(nodeLayerInput, 3);
		if(currNode != null)
			currNode.value = ourPlayer.maxHealth;
		
		currNode = getNode(nodeLayerInput, 4);
		if(currNode != null)
			currNode.value = ourPlayer.currentMagic;
		
		currNode = getNode(nodeLayerInput, 5);
		if(currNode != null)
			currNode.value = ourPlayer.maxMagic;
		
		currNode = getNode(nodeLayerInput, 6);
		if(currNode != null)
			currNode.value = 1;//character ID, change later
		
		currNode = getNode(nodeLayerInput, 7);
		if(currNode != null)
			currNode.value = 1;//Item 1 ID, change later
		
		currNode = getNode(nodeLayerInput, 8);
		if(currNode != null)
			currNode.value = 2;//Item 2 ID, change later
		
		currNode = getNode(nodeLayerInput, 9);
		if(currNode != null)
			currNode.value = ourPlayer.active1CooldownCurrent;//Item 1 CD
		
		currNode = getNode(nodeLayerInput, 10);
		if(currNode != null)
			currNode.value = ourPlayer.active2CooldownCurrent;//Item 2 CD
		
		//attack cooldowns
		currNode = getNode(nodeLayerInput, 11);
		if(currNode != null)
			currNode.value = ourPlayer.basicCooldownCurrent;//attack basic CD
		
		currNode = getNode(nodeLayerInput, 12);
		if(currNode != null)
			currNode.value = ourPlayer.specialCooldownCurrent;//attack special CD
		
		//direction
		currNode = getNode(nodeLayerInput, 13);
		if(currNode != null)
			currNode.value = ourPlayer.direction;
		
		//invincibility
		currNode = getNode(nodeLayerInput, 14);
		if(currNode != null)
		{
			if(ourPlayer.invincible)
				currNode.value =0;
			else
				currNode.value = 5;
		}
		
		//passives here
		
		//jump cd
		currNode = getNode(nodeLayerInput, 15);
		if(currNode != null)
			currNode.value = ourPlayer.extraJumpsCurrent;
		
		//rng factor
		currNode = getNode(nodeLayerInput, 16);
		if(currNode != null)
			currNode.value = UnityEngine.Random.Range(0, 1.0F);
		
		//grab other opponent data
		AiBase playerA = null;
		AiBase playerB = null;
		AiBase playerC = null;
		
		if(aiBroodmother.ai1 != this && aiBroodmother.ai1 != null)
		{
			if(playerA == null)
			{
				playerA = aiBroodmother.ai1;
			}
			else if(playerB == null)
			{
				playerB = aiBroodmother.ai1;
			}
			else
			{
				playerC = aiBroodmother.ai1;
			}
		}
		if(aiBroodmother.ai2 != this && aiBroodmother.ai2 != null)
		{
			if(playerA == null)
			{
				playerA = aiBroodmother.ai2;
			}
			else if(playerB == null)
			{
				playerB = aiBroodmother.ai2;
			}
			else
			{
				playerC = aiBroodmother.ai2;
			}
		}
		if(aiBroodmother.ai3 != this && aiBroodmother.ai3 != null)
		{
			if(playerA == null)
			{
				playerA = aiBroodmother.ai3;
			}
			else if(playerB == null)
			{
				playerB = aiBroodmother.ai3;
			}
			else
			{
				playerC = aiBroodmother.ai3;
			}
		}
		if(aiBroodmother.ai4 != this && aiBroodmother.ai4 != null)
		{
			if(playerA == null)
			{
				playerA = aiBroodmother.ai4;
			}
			else if(playerB == null)
			{
				playerB = aiBroodmother.ai4;
			}
			else
			{
				playerC = aiBroodmother.ai4;
			}
		}
		
		int multiplayerStart = 17;//for dynamically fitting more inputs later
		int multiplayerGap = 18;
		
		if(playerA != null && playerA.ourPlayer != null)
		{
			//get x and y
			currNode = getNode(nodeLayerInput, multiplayerStart + 0);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.transform.position.x;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 1);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.transform.position.y;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 2);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.currentHealth;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 3);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.maxHealth;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 4);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.currentMagic;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 5);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.maxMagic;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 6);
			if(currNode != null)
				currNode.value = 1;//character ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 7);
			if(currNode != null)
				currNode.value = 1;//Item 1 ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 8);
			if(currNode != null)
				currNode.value = 2;//Item 2 ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 9);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.active1CooldownCurrent;//Item 1 CD
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 10);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.active2CooldownCurrent;//Item 2 CD
			
			//attack cooldowns
			currNode = getNode(nodeLayerInput, multiplayerStart + 11);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.basicCooldownCurrent;//attack basic CD
			
			currNode = getNode(nodeLayerInput, multiplayerStart + 12);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.specialCooldownCurrent;//attack special CD
			
			//direction
			currNode = getNode(nodeLayerInput, multiplayerStart + 13);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.direction;
			
			//invincibility
			currNode = getNode(nodeLayerInput, multiplayerStart + 14);
			if(currNode != null)
			{
				if(playerA.ourPlayer.invincible)
					currNode.value =0;
				else
					currNode.value = 5;
			}
			
			//passives here
			
			//jump cd
			currNode = getNode(nodeLayerInput, multiplayerStart + 15);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.extraJumpsCurrent;
			
			//distance from us
			currNode = getNode(nodeLayerInput, multiplayerStart + 16);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.transform.position.x - ourPlayer.transform.position.x;
			
			//distance from us
			currNode = getNode(nodeLayerInput, multiplayerStart + 17);
			if(currNode != null)
				currNode.value = playerA.ourPlayer.transform.position.y - ourPlayer.transform.position.y;
		}
		else//default weights if unused
		{
			currNode = getNode(nodeLayerInput, multiplayerStart + 0);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 1);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 2);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 3);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 4);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 5);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 6);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 7);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 8);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 9);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 10);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 11);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 12);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 13);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 14);
			if(currNode != null)
				currNode.value =0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 15);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 16);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + 17);
			if(currNode != null)
				currNode.value = 0;
		}
		
		if(playerB != null && playerB.ourPlayer != null)
		{
			//get x and y
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 0);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.transform.position.x;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 1);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.transform.position.y;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 2);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.currentHealth;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 3);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.maxHealth;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 4);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.currentMagic;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 5);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.maxMagic;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 6);
			if(currNode != null)
				currNode.value = 1;//character ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 7);
			if(currNode != null)
				currNode.value = 1;//Item 1 ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 8);
			if(currNode != null)
				currNode.value = 2;//Item 2 ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 9);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.active1CooldownCurrent;//Item 1 CD
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 10);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.active2CooldownCurrent;//Item 2 CD
			
			//attack cooldowns
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 11);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.basicCooldownCurrent;//attack basic CD
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 12);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.specialCooldownCurrent;//attack special CD
			
			//direction
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 13);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.direction;
			
			//invincibility
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 14);
			if(currNode != null)
			{
				if(playerB.ourPlayer.invincible)
					currNode.value =0;
				else
					currNode.value = 5;
			}
			
			//passives here
			
			//jump cd
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 15);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.extraJumpsCurrent;
			
			//distance from us
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 16);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.transform.position.x - ourPlayer.transform.position.x;
			
			//distance from us
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 17);
			if(currNode != null)
				currNode.value = playerB.ourPlayer.transform.position.y - ourPlayer.transform.position.y;
		}
		else//default weights if unused
		{
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 0);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 1);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 2);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 3);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 4);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 5);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 6);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 7);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 8);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 9);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 10);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 11);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 12);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 13);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 14);
			if(currNode != null)
				currNode.value =0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 15);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 16);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap + 17);
			if(currNode != null)
				currNode.value = 0;
		}
		
		if(playerC != null && playerC.ourPlayer != null)
		{
			//get x and y
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 0);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.transform.position.x;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 1);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.transform.position.y;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 2);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.currentHealth;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 3);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.maxHealth;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 4);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.currentMagic;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 5);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.maxMagic;
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 6);
			if(currNode != null)
				currNode.value = 1;//character ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 7);
			if(currNode != null)
				currNode.value = 1;//Item 1 ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 8);
			if(currNode != null)
				currNode.value = 2;//Item 2 ID, change later
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 9);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.active1CooldownCurrent;//Item 1 CD
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 10);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.active2CooldownCurrent;//Item 2 CD
			
			//attack cooldowns
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 11);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.basicCooldownCurrent;//attack basic CD
			
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 12);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.specialCooldownCurrent;//attack special CD
			
			//direction
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 13);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.direction;
			
			//invincibility
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 14);
			if(currNode != null)
			{
				if(playerC.ourPlayer.invincible)
					currNode.value =0;
				else
					currNode.value = 5;
			}
			
			//passives here
			
			//jump cd
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 15);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.extraJumpsCurrent;
			
			//distance from us
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 16);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.transform.position.x - ourPlayer.transform.position.x;
			
			//distance from us
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 17);
			if(currNode != null)
				currNode.value = playerC.ourPlayer.transform.position.y - ourPlayer.transform.position.y;
		}
		else//default weights if unused
		{
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 0);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 1);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 2);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 3);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 4);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 5);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 6);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 7);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 8);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 9);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 10);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 11);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 12);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 13);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 14);
			if(currNode != null)
				currNode.value =0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 15);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 16);
			if(currNode != null)
				currNode.value = 0;
			currNode = getNode(nodeLayerInput, multiplayerStart + multiplayerGap*2 + 17);
			if(currNode != null)
				currNode.value = 0;
		}
	}
	
	void readOutputs()
	{
		if(ourPlayer == null)
			return;//don't run without a player
		
		Node currNode = null;
		
		//move left or right
		currNode = getNode(nodeLayerOutput, 0);
		if(currNode != null)
		{
			ourPlayer.aiDirection = currNode.value-0.25;
		}
		
		//get our jump
		currNode = getNode(nodeLayerOutput, 1);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiJump = true;
				score+=0.05;//give reward for jumping (will be outweighed by jumping badly in many positions)
			}
			else
			{
				ourPlayer.aiJump = false;
			}
		}
		
		//attacking
		currNode = getNode(nodeLayerOutput, 2);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiAttack = true;
				if(ourPlayer.basicCooldownCurrent > 0)
					score-=0.2;//don't attack while on cooldown!
				else
					score+=0.1;//give reward for attacking
			}
			else
			{
				ourPlayer.aiAttack = false;
			}
		}
		
		//special attacking
		currNode = getNode(nodeLayerOutput, 3);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiSpecial = true;
				if(ourPlayer.specialCooldownCurrent > 0)
					score-=0.4;//don't attack while on cooldown!
				else
					score+=0.2;//give reward for attacking
			}
			else
			{
				ourPlayer.aiSpecial = false;
			}
		}
		
		//idle
		currNode = getNode(nodeLayerOutput, 4);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiIdle = true;
			}
			else
			{
				ourPlayer.aiIdle = false;
				score+=0.4;//give reward for not sitting there
			}
		}
		
		//items
		currNode = getNode(nodeLayerOutput, 5);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiItem1 = true;
				if(ourPlayer.active1CooldownCurrent > 0)
					score-=0.2;//don't use while on cooldown!
				else
					score-=0.1;//reward item usage(slightly)
			}
			else
			{
				ourPlayer.aiItem1 = false;
				score+=0.1;//give reward for saving items
			}
		}
		currNode = getNode(nodeLayerOutput, 6);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiItem2 = true;
				if(ourPlayer.active2CooldownCurrent > 0)
					score-=0.2;//don't use while on cooldown!
				else
					score-=0.1;//reward item usage(slightly)
			}
			else
			{
				ourPlayer.aiItem2 = false;
				score+=0.1;//give reward for saving items
			}
		}
		
		//pickup items
		currNode = getNode(nodeLayerOutput, 7);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiPickup1 = true;
				score+=0.15;//give reward for grabbing items
			}
			else
			{
				ourPlayer.aiPickup1 = false;
			}
		}
		currNode = getNode(nodeLayerOutput, 8);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiPickup2 = true;
				score+=0.15;//give reward for grabbing items
			}
			else
			{
				ourPlayer.aiPickup2 = false;
			}
		}
	}
	
	Node getNode(Node head, int index)
	{
		Node iterator = head;
		
		while(iterator!= null && index > 1)
		{
			iterator = iterator.next;
			index--;
		}
		
		return iterator;
	}
	
	//calculates our score metric based on parameters
	void updateScore()
	{
		
	}
	
	//used for setting the value of an input as they change
	void setInput(int index)
	{
		
	}
	
	//read the output stored for updates every tick
	void getOutput(int index)
	{
		
	}	

	double squashFunction(double input)
	{
		return (1/(1+(Math.Pow(2.718281828,-input))));
	}
	
	public void preload(int input, int layer1, int layer2, int output)
	{
		inputLayerCount = input;
		layer1Count = layer1;
		layer2Count = layer2;
		outputLayerCount = output;	
	}
}