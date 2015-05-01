using UnityEngine;
using System.Collections;

public class AnimatorSoldier : CustomAnimator
{
    private string currentAnim;
    
    public override void LateUpdate ()
    {
        if (animator != null)
        {
            if (state == "defeated")
            {
                if (currentAnim != "Defeated")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Hurt_Air";
                    else
                        currentAnim = "Defeated";
                }
                
            }
            else if (state == "hurt")
            {
                if (currentAnim != "Hurt_Air" && currentAnim != "Hurt_Ground")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Hurt_Ground";
                    else
                        currentAnim = "Hurt_Air";
                }
            }
            else if (state == "basic start")
            {
                if (currentAnim != "Attack_Air_Start" && currentAnim != "Attack_Ground_Start")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Attack_Air_Start";
                    else
                        currentAnim = "Attack_Ground_Start";
                }
            }
            else if (state == "basic")
            {
                if (currentAnim != "Attack_Air" && currentAnim != "Attack_Ground")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Attack_Air";
                    else
                        currentAnim = "Attack_Ground";
                }
            }
            else if (state == "basic end")
            {
                if (currentAnim != "Attack_Air_End" && currentAnim != "Attack_Ground_End")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Attack_Air_End";
                    else
                        currentAnim = "Attack_Ground_End";
                }
            }
            else if (state == "special start")
            {
                currentAnim = "Special_Start";
            }
            else if (state == "special")
            {
                currentAnim = "Special";
            }
            else if (state == "special end")
            {
                currentAnim = "Special_End";
            }
            else if (!physics.collideBottom)
            {
                if (physics.speed.y > 2)
                    currentAnim = "Jump_Up";
                else if (physics.speed.y < -2)
                    currentAnim = "Jump_Down";
                else
                    currentAnim = "Jump_Neutral";
            }
            else if (physics.speed.x != 0)
            {
                currentAnim = "Running";
            }
            else
            {
                currentAnim = "Idle";
            }
            animator.Play(currentAnim);
        }
        base.LateUpdate();
    }
}
