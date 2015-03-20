using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBattleInterfaceScript : MonoBehaviour
{
	public ControllerHero connectedPlayer = null;
	
    private Sprite[] healthSpriteArray;
    private Sprite[] manaSpriteArray;
	
	CanvasGroup ourCanvas = null;

	// Use this for initialization
	void Start ()
	{
		ourCanvas = this.GetComponent<CanvasGroup>();
		
		if(connectedPlayer == null)
		{
			//hide ourself
			
			ourCanvas.alpha = 0;
			
			//abort if we don't have player data
			return;
		}
		else
		{
			healthSpriteArray = Resources.LoadAll<Sprite> ("health");
			manaSpriteArray = Resources.LoadAll<Sprite> ("mana");
			
			Component[] texts = GetComponentsInChildren<Text>();
			//nameField = Text.Find("NameField");

			foreach (Text e in texts)
			{
				//if (e.name == "HealthField")
					//e.text = ""+connectedPlayer.currentHealth;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(connectedPlayer != null)
		{
			ourCanvas.alpha = 1;
			
			Component[] images = GetComponentsInChildren<Image>();
			//nameField = Text.Find("NameField");

			foreach (Image e in images)
			{
				if (e.name == "HealthHeart1")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.166)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.084)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart2")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.33)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.25)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart3")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.5)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.413)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart4")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.66)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.58)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart5")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.83)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.75)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart6")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 1.0)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth / connectedPlayer.maxHealth >= 0.91)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				
				else if (e.name == "ManaBottle1")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.166)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.084)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle2")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.33)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.25)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle3")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.5)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.413)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle4")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.66)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.58)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle5")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.83)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.75)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle6")
				{
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 1.0)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic / connectedPlayer.maxMagic >= 0.91)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
			}
		}
		else
		{
			ourCanvas.alpha = 0;
		}
	}
}
