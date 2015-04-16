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
	int inputLayerCount = 102;
	Node nodeLayer1 = null;//20 of these
	int layer1Count = 20;
	Node nodeLayer2 = null;//20 of these
	int layer2Count = 20;
	Node nodeLayerOutput = null;
	int outputLayerCount = 9;
		
	double score = 0;//this is our performance with this genetic layout, higher score = better
		//modified during gameplay at real-time
	
	//scoring data
		//use this data for calculating the score, such as trickshots
		//may be useful for stats screen?
	
	protected double aiLastDirection = 0;

    void Start ()
    {
		//string text = System.IO.File.ReadAllText("myfile.txt");
		System.IO.File.WriteAllText("AI/weights_", "7 microhitlers\ntesting");
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
			
			//do not touch it if we aren't able to do anything
			//proc movement to the left just because
			if(ourPlayer.aiIdle)
				aiLastDirection = 0;
			else
				aiLastDirection = ourPlayer.aiDirection;
			
			//check the outputs of calculation
			
			//ourPlayer.aiIdle = false;
			
			//ourPlayer.aiDirection = Random.Range(-10.0F, 10.0F);
			//ourPlayer.aiJump = true;
			//ourPlayer.aiAttack = true;
			//ourPlayer.aiSpecial = true;
			
			//update the game status
			
			//run through our network
			
			//set our outputs
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
		//Our passives
		//Our cooldowns
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
			currNode.value = 0;//Item 1 CD, change later
		
		currNode = getNode(nodeLayerInput, 10);
		if(currNode != null)
			currNode.value = 0;//Item 2 CD, change later
		
		//attack cooldowns
		
		//passives here
		
		//jump cd
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
				score+=1;//give reward for jumping (will be outweighed by jumping badly in many positions)
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
				score+=1;//give reward for attacking
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
				score+=1;//give reward for attacking
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
				score+=1;//give reward for not sitting there
			}
		}
		
		//items
		currNode = getNode(nodeLayerOutput, 5);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiItem1 = true;
			}
			else
			{
				ourPlayer.aiItem1 = false;
				score+=1;//give reward for saving items
			}
		}
		currNode = getNode(nodeLayerOutput, 6);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiItem2 = true;
			}
			else
			{
				ourPlayer.aiItem2 = false;
				score+=1;//give reward for saving items
			}
		}
		
		//pickup items
		currNode = getNode(nodeLayerOutput, 7);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiPickup1 = true;
				score+=1;//give reward for grabbing items
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
				score+=1;//give reward for grabbing items
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
}