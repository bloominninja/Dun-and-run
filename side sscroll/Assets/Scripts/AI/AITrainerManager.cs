using UnityEngine;
using System.Collections;
using System;


public class AiTrainerManager
{
	//each of the AI we are responsible for
	//we load in data for the four of them to train
	//to reset, simply replace the data
	public AiBase ai1 = null;
	public AiBase ai2 = null;
	public AiBase ai3 = null;
	public AiBase ai4 = null;
	
	public bool trainingMode = true;
	
	public GameManager gameManager = null;
	
	//this is used to denote what stage of the mode is active
	public string externalStage = "Training";
	int currentGenome = 1;
	const int maxGenome = 64;
	int currentSet = 0;
	int totalIterations = 1;
	
	int genomesPerSet = 4;
	
	const double timerReset = 30;
	
	double lastTime = timerReset;
	double timer = 0;//timerReset seconds per cycle default
	
	ArrayList[] inputToLayer1Genes = new ArrayList[maxGenome];
	ArrayList[] layer1ToLayer2Genes = new ArrayList[maxGenome];
	ArrayList[] layer2ToOutputGenes = new ArrayList[maxGenome];
	
	//for our reference
	int inputLayerCount = 102;
	int layer1Count = 20;
	int layer2Count = 20;
	int outputLayerCount = 8;

	// Use this for initialization
	public void Start ()
	{
		//create the genome storage
		for(int i=0; i<maxGenome; i++)
		{
			inputToLayer1Genes[i] = new ArrayList();
			layer1ToLayer2Genes[i] = new ArrayList();
			layer2ToOutputGenes[i] = new ArrayList();
		}
		
		//generate the data (random for now)
		for(int i=0; i<maxGenome; i++)
		{
			for(int j=0; j<inputLayerCount*layer1Count; j++)
			{
				inputToLayer1Genes[i].Add(UnityEngine.Random.Range(-10.0F, 10.0F));
			}
			for(int j=0; j<layer1Count*layer2Count; j++)
			{
				layer1ToLayer2Genes[i].Add(UnityEngine.Random.Range(-10.0F, 10.0F));
			}
			for(int j=0; j<layer2Count*outputLayerCount; j++)
			{
				layer2ToOutputGenes[i].Add(UnityEngine.Random.Range(-10.0F, 10.0F));
			}
		}
	}
	
	public void OnDestroy()
	{
		//store our genomes quickly!
		
		
		//destroy the genome storage
		for(int i=0; i<maxGenome; i++)
		{
			inputToLayer1Genes[i].Clear();
			layer1ToLayer2Genes[i].Clear();
			layer2ToOutputGenes[i].Clear();
		}
	}
	
	// Update is called once per frame
	public void Update ()
	{
		//get the match state
		//do not tick if there is no match
		if(gameManager.stage != null && !gameManager.end)
		{
			timer -= Time.deltaTime;
			
			if(lastTime > Math.Floor(timer))
			{
				Debug.Log("AI Timer: "+(lastTime.ToString())+" of set "+(currentSet.ToString())+". Iteration "+(totalIterations.ToString()));
				lastTime = Math.Floor(timer);
			}
			
			//check if the timer is up
			if(timer < 0)
			{
				gameManager.end = true;//we force the match to end
				
				//reset our timer
				timer = timerReset;
				lastTime = timerReset;
				
				//make sure to step our genetic process here!
				currentSet++;
				
				//attempt to load in all of the weights for this set to our precious little AI
				
				if(currentSet*genomesPerSet > maxGenome)
				{
					//mix genetics here
					
					//reset our stats
					currentSet = 1;
					totalIterations++;
					currentGenome = 1;
				}
			}
		}
	}
	
	//here we select the next generation that we will be testing
	public void adjustGeneticPool()
	{
		//keep min(25%, # with score>threshold)
		//we don't want the weak staying in our gene pool, might as well re-random it
		
		//for each slot being replaced (all if none >threshold), genetic a new set of weights (>=25% of pool)
		
		//if no genetics kept, return here, we're done
		
		
		//if here, then some scores didn't suck horribly
		//top 25% for breeding
		//50% used for children of breeding pool of varying mutation
			//12.5% for high mutation
			//12.5% for medium mutation
			//12.5% for low mutation
			//12.5% for genetic segment replacement (with top 25)
				//33% 5% replacement
				//33% 10% replacement
				//33% 1% replacement
				
		//keep original 25% top
		
		//shuffle our gene sets so that they will test in random groupings
	}
	
	//the shuffle algorithm for shuffling our genetic sets
	public void shuffleGeneticPool()
	{
		
	}
	
	public void linkAI(AiBase playerAI, int index)
	{
		switch(index)
		{
			case 1:
				ai1 = playerAI;
				break;
			case 2:
				ai2 = playerAI;
				break;
			case 3:
				ai3 = playerAI;
				break;
			case 4:
				ai4 = playerAI;
				break;
		}
	}
	
}
