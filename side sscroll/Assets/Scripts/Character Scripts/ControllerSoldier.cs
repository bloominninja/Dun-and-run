using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorSoldier))]
public class ControllerSoldier : PlayerController
{
    public MeleeProjectile projBasicLeft, projBasicRight, projSpecialLeft, projSpecialRight;
    
    protected float basicDelay, basicDuration, specialDelay, specialDuration = -1;
    
    protected override void Start ()
    {
        base.Start();
        basicCooldown = 0.3f;
        specialCooldown = 2;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projSpecialLeft.team = team;
        projSpecialRight.team = team;
        projBasicLeft.direction = -1;
        projSpecialLeft.direction = -1;
        
        extraJumps += 1;
    }
    
    protected void LateUpdate ()
    {
        if (GameManager.o.pause)
            return;
        
        if (basicDelay > 0)
        {
            basicDelay -= Time.deltaTime;
            if (basicDelay <= 0)
            {
                animator.state = "basic";
                if (direction > 0)
                    projBasicRight.Activate(0.3f);
                else
                    projBasicLeft.Activate(0.3f);
                physics.SetSpeedX(10 * direction, 0.2f);
                physics.SetSpeedY(-0.01f, 0.2f);
                basicDuration = 0.2f;
            }
        }
        else if (basicDuration > 0)
        {
            basicDuration -= Time.deltaTime;
            physics.SetSpeedX(10 * direction, 0);
            if (basicDuration <= 0)
            {
                animator.state = "basic end";
                physics.SetSpeedX(0, 0);
            }
        }
        
        if (specialDelay > 0)
        {
            specialCooldownCurrent = specialCooldown;
            specialDelay -= Time.deltaTime;
            if (specialDelay <= 0)
            {
                animator.state = "special";
                if (direction > 0)
                    projSpecialRight.Activate(20);
                else
                    projSpecialLeft.Activate(20);
                projSpecialLeft.damage = 1;
                projSpecialRight.damage = 1;
                physics.SetSpeedX(4 * direction, 20);
                physics.SetSpeedY(-physics.gravity, 20);
                specialDuration = 0;
                LockInput(1);
            }
        }
        else if (specialDuration >= 0)
        {
            specialCooldownCurrent = specialCooldown;
            specialDuration += Time.deltaTime;
            if (physics.collideBottom)
            {
                projSpecialLeft.Deactivate();
                projSpecialRight.Deactivate();
                animator.state = "special end";
                physics.SetSpeedX(0, 0.3f);
                physics.SetSpeedY(0, 0.3f);
                LockInput(0.3f);
                specialDuration = -1;
            }
            else if (specialDuration >= 0.7f)
            {
                projSpecialLeft.damage = 2;
                projSpecialRight.damage = 2;
            }
        }
        
    }
    
    protected override void AttackEffect ()
    {
        if (basicCooldownCurrent <= 0)
        {
            basicCooldownCurrent = basicCooldown;
            basicDelay = 0.1f;
            
            animator.state = "basic start";
            LockInput(0.5f);
        }
    }
    
    protected override void SpecialEffect ()
    {
        if (currentMagic >= 2 && specialCooldownCurrent <= 0)
        {
            specialCooldownCurrent = specialCooldown;
            currentMagic -= 2;
            specialDelay = 0.1f;
            
            animator.state = "special start";
            LockInput(5f);
        }
    }
    
    public override void Damage (int damage, int dir, PlayerController source)
    {
        base.Damage(damage, dir, source);
        projBasicLeft.Deactivate();
        projBasicRight.Deactivate();
        projSpecialLeft.Deactivate();
        projSpecialRight.Deactivate();
        basicDelay = 0;
        basicDuration = 0;
        specialDelay = 0;
        specialDuration = -1;
    }
    
    public override void Bounce (GameObject other, Vector2 sp)
    {
        if (specialDuration >= 0)
        {
            if (other.GetComponent<PlayerController>().physics.collideBottom && sp.y != 0)
            {
                projSpecialLeft.Deactivate();
                projSpecialRight.Deactivate();
                animator.state = "special end";
                physics.SetSpeedY(sp.y, 0.3f);
                LockInput(0.3f);
                specialDuration = -1;
            }
            else
                return;
        }
        if (sp.x != 0)
        {
            physics.SetSpeedX(sp.x, 0.2f);
        }
        else if (sp.y != 0)
        {
            if (physics.collideBottom && sp.y < 0)
                physics.SetSpeedX(sp.y * -1 * Mathf.Sign(transform.position.x - other.transform.position.x), 0.2f);
            else
                physics.SetSpeedY(sp.y, 0.2f);
        }
    }
}
