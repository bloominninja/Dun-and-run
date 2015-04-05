using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiBase : MonoBehaviour
{
    public PlayerController ourPlayer;

    void Start ()
    {
    }
	
    void Update ()
    {
		if(ourPlayer!=null)
		{
			//do not touch it if we aren't able to do anything
			//proc movement to the left just because
			ourPlayer.aiDirection = Random.Range(-10.0F, 10.0F);
			ourPlayer.aiJump = true;
		}
	}
}