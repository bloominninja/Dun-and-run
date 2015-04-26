using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AnimatorHero))]
public class ControllerHero : PlayerController
{
    public ProjectileHeroArrow projArrow;
    public ProjectileHeroBasic projBasicLeft, projBasicRight;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        maxMagic = 8;
        currentMagic = 8;
        basicCooldown = 0.3f;
        specialCooldown = 1.8f;
        projBasicLeft.team = team;
        projBasicRight.team = team;
        projBasicLeft.direction = -1;
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

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
            animator.attacking = true;
            LockInput(0.2f);
        }
    }

    protected override void SpecialEffect ()
    {
        if (currentMagic >= 1 && specialCooldownCurrent <= 0)
        {
            currentMagic -= 1;
            specialCooldownCurrent = specialCooldown;

            ProjectileHeroArrow t = (ProjectileHeroArrow)Instantiate(projArrow);
            t.transform.position = transform.position + (Vector3)box.offset;
            t.direction = direction;
            t.team = team;

            animator.special = true;
            LockInput(0.3f);
        }
    }

    public override void Damage (int damage, int dir, PlayerController source)
    {
        base.Damage(damage, dir, source);
        animator.attacking = false;
        animator.special = false;
        projBasicLeft.Deactivate();
        projBasicRight.Deactivate();
    }

    public override void UnlockInput ()
    {
        base.UnlockInput();
        animator.attacking = false;
        animator.special = false;
    }
}
