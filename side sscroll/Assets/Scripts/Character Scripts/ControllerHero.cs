using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorHero))]
public class ControllerHero : PlayerController
{
    public ProjectileHeroArrow projArrow;
    public MeleeProjectile projBasicLeft, projBasicRight;
    
    public bool specialCharge;
    
    protected override void Start ()
    {
        base.Start();
        basicCooldown = 0.6f;
        specialCooldown = 1.8f;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projBasicLeft.direction = -1;
        specialChargeTimeMax = 0.7f;
    }
    
    protected void LateUpdate ()
    {
        if (GameManager.o.pause)
            return;
        
        if (active1 != null && active1CooldownCurrent > 0)
            active1CooldownCurrent -= Time.deltaTime * 0.25f;
        if (active2 != null && active2CooldownCurrent > 0)
            active2CooldownCurrent -= Time.deltaTime * 0.25f;
        
        if (specialCharge)
        {
            specialChargeTime += Time.deltaTime;
            if (specialChargeTime > specialChargeTimeMax)
                specialChargeTime = specialChargeTimeMax;
            if (Special())
            {
                LockInput(0.1f);
                animator.state = "special";
            }
            else
            {
                specialCharge = false;
                if (currentMagic >= 1)
                {
                    specialCooldownCurrent = specialCooldown;
                    animator.state = "special fire";
                    LockInput(0.3f);
                    
                    ProjectileHeroArrow t = (ProjectileHeroArrow)Instantiate(projArrow);
                    t.transform.position = transform.position + (Vector3)box.offset;
                    t.direction = direction;
                    t.team = team;
                    
                    if (specialChargeTime == specialChargeTimeMax && currentMagic >= 2)
                    {
                        currentMagic -= 2;
                        t.speed = t.speed * 2;
                        t.dropTime = 5;
                    }
                    else
                    {
                        currentMagic -= 1;
                        t.speed = t.speed * (1 + (0.1f * Mathf.Floor(6 * (specialChargeTime / specialChargeTimeMax))));
                        t.dropTime = t.dropTime * (1 + (0.1f * Mathf.Floor(6 * (specialChargeTime / specialChargeTimeMax))));
                    }
                }
            }
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
            physics.speed.x += 6 * direction;
            animator.state = "attacking";
            LockInput(0.2f);
        }
    }
    
    protected override void SpecialEffect ()
    {
        if (currentMagic >= 1 && specialCooldownCurrent <= 0)
        {
            specialChargeTime = 0;
            specialCharge = true;
            animator.state = "special";
            LockInput(0);
        }
    }
    
    public override void Damage (int damage, int dir, PlayerController source)
    {
        base.Damage(damage, dir, source);
        projBasicLeft.Deactivate();
        projBasicRight.Deactivate();
        specialCharge = false;
    }
}
