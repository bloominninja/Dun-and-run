using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorPrincess))]
public class ControllerPrincess : PlayerController
{
    public MeleeProjectile projBasicLeft, projBasicRight, projChargeLeft, projChargeRight;
    
    public bool basicCharge;
    
    protected override void Start ()
    {
        base.Start();
        basicCooldown = 0.3f;
        specialCooldown = 2.5f;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projChargeLeft.team = team;
        projChargeRight.team = team;
        projBasicLeft.direction = -1;
        projChargeLeft.direction = -1;
        basicChargeTimeMax = 0.7f;
    }
    
    protected void LateUpdate ()
    {
        if (GameManager.o.pause)
            return;
        
        if (basicCharge)
        {
            basicChargeTime += Time.deltaTime;
            if (basicChargeTime > basicChargeTimeMax)
                basicChargeTime = basicChargeTimeMax;
            if (!Attack())
            {
                basicCharge = false;
                if (basicChargeTime == basicChargeTimeMax && currentMagic >= 1 && !inputLock)
                {
                    LockInput(0.4f);
                    currentMagic -= 1;
                    animator.state = "charge";
                    physics.SetSpeedX(25 * direction, 0.2f, 0);
                    physics.SetSpeedY(0, 0.2f);
                    if (direction > 0)
                        projChargeRight.Activate(0.2f);
                    else
                        projChargeLeft.Activate(0.2f);
                }
            }
        }
        else
        {
            basicChargeTime = 0;
            basicCharge = true;
        }
    }
    
    protected override void AttackEffect ()
    {
        if (basicCooldownCurrent <= 0)
        {
            basicCooldownCurrent = basicCooldown;
            
            if (direction > 0)
                projBasicRight.Activate(0.2f);
            else
                projBasicLeft.Activate(0.2f);
            physics.speed.x += 9 * direction;
            animator.state = "basic";
            LockInput(0.4f);
        }
    }
    
    protected override void SpecialEffect ()
    {
        if (currentMagic >= 1 && specialCooldownCurrent <= 0)
        {
            if (Left())
                direction = -1;
            else if (Right())
                direction = 1;
            
            currentMagic -= 1;
            specialCooldownCurrent = specialCooldown;
            physics.SetSpeedX(11 * direction, 0.5f);
            physics.SetSpeedY(jumpHeight / 2, 0);
            SetInvincible(0.5f);
            LockInput(0.5f);
            animator.state = "special";
            physics.collLayer2 = null;
            gameObject.layer = 3;
            Debug.Log(gameObject.layer.ToString());
        }
    }
    
    public override void Damage (int damage, int dir, PlayerController source)
    {
        base.Damage(damage, dir, source);
        projBasicLeft.Deactivate();
        projBasicRight.Deactivate();
        projChargeLeft.Deactivate();
        projChargeRight.Deactivate();
        basicCharge = false;
    }
    
    public override void UnlockInput ()
    {
        base.UnlockInput();
        physics.collLayer2 = "Characters";
        gameObject.layer = 9;
    }
}
