using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiBase : MonoBehaviour
{
    public PlayerController ourPlayer;

    void Start ()
    {
		//string text = System.IO.File.ReadAllText("myfile.txt");
		System.IO.File.WriteAllText("AI/weights_", "7 microhitlers\ntesting");
    }
	
    void Update ()
    {
		if(ourPlayer!=null)
		{
			//do not touch it if we aren't able to do anything
			//proc movement to the left just because
			ourPlayer.aiLastDirection = ourPlayer.aiDirection;
			ourPlayer.aiDirection = Random.Range(-10.0F, 10.0F);
			ourPlayer.aiJump = true;
			ourPlayer.aiAttack = true;
			ourPlayer.aiSpecial = true;
		}
	}
}