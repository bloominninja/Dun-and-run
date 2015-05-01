using UnityEngine;
using System.Collections;

public class AnimatorHero : CustomAnimator
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
                        currentAnim = "Hurt_2";
                    else
                        currentAnim = "Defeated";
                }
                
            }
            else if (state == "hurt")
            {
                if (currentAnim != "Hurt_2" && currentAnim != "Hurt_1")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Hurt_1";
                    else
                        currentAnim = "Hurt_2";
                }
            }
            else if (state == "attacking")
            {
                if (currentAnim != "Attack_2" && currentAnim != "Attack_1")
                {
                    if (!physics.collideBottom)
                        currentAnim = "Attack_2";
                    else
                        currentAnim = "Attack_1";
                }
            }
            else if (state == "special fire")
            {
                currentAnim = "Special_Fire";
            }
            else if (state == "special")
            {
                currentAnim = "Special";
            }
            else if (!physics.collideBottom)
            {
                if (physics.speed.y > 2)
                    currentAnim = "Jump_up";
                else if (physics.speed.y < -2)
                    currentAnim = "Jump_down";
                else
                    currentAnim = "Jump_neutral";
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

        if (state == "special" && player.specialChargeTime == player.specialChargeTimeMax)
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
