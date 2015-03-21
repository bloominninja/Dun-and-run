﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBattleInterfaceScript : MonoBehaviour
{
	public ControllerHero connectedPlayer = null;
	
    public Sprite[] healthSpriteArray;
    public Sprite[] manaSpriteArray;
	
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
			
			//Component[] texts = GetComponentsInChildren<Text>();
			//nameField = Text.Find("NameField");

			//foreach (Text e in texts)
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
                    if(connectedPlayer.maxHealth < 2) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth >=2)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth == 1)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart2")
                {
                    if(connectedPlayer.maxHealth < 4) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth  >= 4)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth == 3)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart3")
                {
                    if(connectedPlayer.maxHealth < 6) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth >= 6)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth ==5)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart4")
                {
                    if(connectedPlayer.maxHealth < 8) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth >=8)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth ==7)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart5")
                {
                    if(connectedPlayer.maxHealth < 10) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth >=10)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth ==9)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				else if (e.name == "HealthHeart6")
                {
                    if(connectedPlayer.maxHealth < 12) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentHealth >=12)
						e.sprite = healthSpriteArray[0];
					else if((double) connectedPlayer.currentHealth ==11)
						e.sprite = healthSpriteArray[1];
					else
						e.sprite = healthSpriteArray[2];
				}
				
				else if (e.name == "ManaBottle1")
                {
                    if(connectedPlayer.maxMagic < 2) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic >=2)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic ==1)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle2")
                {
                    if(connectedPlayer.maxMagic < 4) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic >=4)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic ==3)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle3")
                {
                    if(connectedPlayer.maxMagic < 6) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic >=6)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic ==5)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle4")
                {
                    if(connectedPlayer.maxMagic < 8) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic >=8)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic ==7)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle5")
                {
                    if(connectedPlayer.maxMagic < 10) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic >=10)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic ==9)
						e.sprite = manaSpriteArray[1];
					else
						e.sprite = manaSpriteArray[2];
				}
				else if (e.name == "ManaBottle6")
                {
                    if(connectedPlayer.maxMagic < 12) e.enabled = false;
                    else e.enabled = true;
					//check if health is above a threshold for current health
					if((double) connectedPlayer.currentMagic >=12)
						e.sprite = manaSpriteArray[0];
					else if((double) connectedPlayer.currentMagic ==11)
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
