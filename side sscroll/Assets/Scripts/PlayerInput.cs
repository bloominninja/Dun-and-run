using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerInput : MonoBehaviour
{
	//Player Handling
	//Public Values
	public float speed = 8;
	public float acceleration = 12;
	public float gravity = 20;
	public float jumpHeight = 12;
	public float jumps = 1;
	//Private Values
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float RJumps;
	private PlayerPhysics playerPhysics;
	private bool right;
	private bool wallJump;

	//input string variables, use any of the strings in the Input Manager
	//can be changed on the fly to rebind keys and set up control devices
	public string H = "Horizontal";
	public string V = "Vertical";
	public string Jump = "Jump";
	public string Basic = "Fire1";
	public string Special;
	public string Item1;
	public string Item2;
	public string Grab1;
	public string Grab2;

	void Start ()
	{

		playerPhysics = GetComponent<PlayerPhysics> ();
		//right=true;
		
	}
	
	
	void Update ()
	{
		targetSpeed = Input.GetAxisRaw (H) * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);

		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			RJumps = jumps;
			wallJump = false;
		}	
		if (playerPhysics.stopMove) {
			if (currentSpeed > 0) {
				right = true;
				Debug.Log ("right");
			} else if (currentSpeed < 0) {
				right = false;
				Debug.Log ("left");
			}
			if (!playerPhysics.grounded) {
				wallJump = true;
			}
			targetSpeed = 0;
			currentSpeed = 0;

		}
		if (Input.GetButtonDown (Jump)) {

			if (wallJump) {
				if (right) {
					currentSpeed = -18;
					right = false;
				} else {
					currentSpeed = 18;
					right = true;
				}
				amountToMove.y = jumpHeight;

			}
			if ((RJumps > 0) && wallJump == false) {
				amountToMove.y = jumpHeight;
				RJumps -= 1;
			}
			wallJump = false;
		}

		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);
	}
	
	private float IncrementTowards (float n, float target, float a)
	{
		if (n == target) {
			return n;
		} else {
			float dir = Mathf.Sign (target - n);
			n += a * Time.deltaTime * dir;
			
			if (dir == Mathf.Sign (target - n)) {
				return n;
			} else {
				return target;
			}
		}
	}
	
}
