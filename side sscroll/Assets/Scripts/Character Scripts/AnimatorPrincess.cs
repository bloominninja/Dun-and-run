using UnityEngine;
using System.Collections;

public class AnimatorPrincess : CustomAnimator
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
            else if (state == "special")
            {
                currentAnim = "Roll";
            }
            else if (state == "charge")
            {
                currentAnim = "Charge_Attack";
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
        
        if (player.basicChargeTime == player.basicChargeTimeMax)
        {
            whiteTimer -= Time.deltaTime;
            if (whiteTimer <= 0)
            {
                whiteToggle = !whiteToggle;
                opacityTimer = 0.2f;
            }
        }
        else
        {
            whiteTimer = 0.2f;
            whiteToggle = false;
        }
        
        
        if (whiteToggle)
        {
            foreach (Sprite s in whiteSheet)
            {
                if (s.name == rend.sprite.name)
                {
                    rend.sprite = s;
                    break;
                }
            }
        }
    }
}
