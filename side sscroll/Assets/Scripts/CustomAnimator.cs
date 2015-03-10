using UnityEngine;
using System.Collections;

public class CustomAnimator : MonoBehaviour
{

    public Animation animations;
    protected CustomPhysics physics;
    protected PlayerController player;

    // Use this for initialization
    void Start ()
    {
        animations = GetComponentInChildren<Animation>();
        physics = GetComponent<CustomPhysics>();
        player = GetComponent<PlayerController>();
    }
	
    // Update is called once per frame
    void LateUpdate ()
    {
        if (animations != null)
        {
            if (player.direction > 0)
                transform.eulerAngles = Vector3.zero;
            else if (player.direction < 0)
                transform.eulerAngles = Vector3.up * 180;

            if (player.attacking)
            {
                animations.Play("attack");
                animations.wrapMode = WrapMode.Once;
            }
            else if (!physics.collideBottom)
            {
                animations.Play("jump");
                animations.wrapMode = WrapMode.Once;
            }
            else if (physics.speed.x != 0)
            {
                animations.Play("run");
                animations.wrapMode = WrapMode.Loop;
            }
            else
            {
                animations.Play("idle");
                animations.wrapMode = WrapMode.Loop;
            }
        }
    }
}
