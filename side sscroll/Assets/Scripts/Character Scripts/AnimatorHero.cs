using UnityEngine;
using System.Collections;

public class AnimatorHero : CustomAnimator
{

    // Use this for initialization
    public override void Start ()
    {
        base.Start();

    }

    public override void LateUpdate ()
    {
        base.LateUpdate();
        if (animator != null)
        {
            if (hurt)
            {
                if (!physics.collideBottom)
                    animator.Play("Hurt_1");
                else
                    animator.Play("Hurt_2");
            }
            else if (attacking)
            {
                if (!physics.collideBottom)
                    animator.Play("Attack_2");
                else
                    animator.Play("Attack_1");
            }
            else if (special)
            {
                animator.Play("Special");
            }
            else if (!physics.collideBottom)
            {
                if (physics.speed.y > 2)
                    animator.Play("Jump_up");
                else if (physics.speed.y < -2)
                    animator.Play("Jump_down");
                else
                    animator.Play("Jump_neutral");
            }
            else if (physics.speed.x != 0)
            {
                animator.Play("Running");
            }
            else
            {
                animator.Play("Idle");
            }
        }
    }
}
