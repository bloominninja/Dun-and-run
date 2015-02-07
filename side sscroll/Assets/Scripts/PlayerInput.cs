using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerInput : MonoBehaviour 
{
	//Player Handling
		//Public Values
			public float speed=8;
			public float acceleration=12;
			public float gravity=20;
			public float jumpHeight=12;
			public float jumps=1;
		//Private Values
			private float currentSpeed;
			private float targetSpeed;
			private Vector2 amountToMove;
			private float RJumps;
			private PlayerPhysics playerPhysics;
	

	void Start () {

		playerPhysics=GetComponent<PlayerPhysics>();

	}
	
	
	void Update () 
	{
		targetSpeed=Input.GetAxisRaw("Horizontal")*speed;
		currentSpeed =IncrementTowards(currentSpeed,targetSpeed,acceleration);
		if(playerPhysics.grounded)
		{
			amountToMove.y=0;
			RJumps=jumps;
		}	
		if(Input.GetButtonDown ("Jump")&&RJumps>0)
			{
				amountToMove.y=jumpHeight;
				RJumps-=1;
		}

		amountToMove.x=currentSpeed;
		amountToMove.y-=gravity*Time.deltaTime;
		playerPhysics.Move (amountToMove*Time.deltaTime);
	}
	
	private float IncrementTowards(float n, float target, float a)
	{
		if(n==target)
		{
			return n;
		}
		else
		{
			float dir=Mathf.Sign (target-n);
			n+= a*Time.deltaTime*dir;
			
			if(dir==Mathf.Sign (target-n))
			{
				return n;
			}
			else
			{
				return target;
			}
		}
	}
}
