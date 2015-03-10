using UnityEngine;
using System.Collections;

public class ControllerHero : PlayerController
{
    public float attackTime = 0;
    public ProjectileHeroArrow projArrow;
    [HideInInspector]
    public ProjectileHeroBasic
        projBasic;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        maxMagic = 8;
        currentMagic = 8;
        basicCooldown = 1.5f;
        specialCooldown = 2;
        projBasic = GetComponentInChildren<ProjectileHeroBasic>();
        projBasic.team = team;
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        if (attacking)
        {
            attackTime -= Time.deltaTime;
            if (attackTime <= 0)
            {
                attacking = false;
                projBasic.on = false;
            }
        }
        base.Update();

    }

    protected override void AttackEffect ()
    {
        if (basicCooldownCurrent <= 0)
        {
            basicCooldownCurrent = basicCooldown;

            projBasic.on = true;
            attacking = true;
            attackTime = 0.5f;
            LockInput(0.5f);
        }
    }

    protected override void SpecialEffect ()
    {
        if (currentMagic >= 1 && specialCooldownCurrent <= 0)
        {
            currentMagic -= 1;
            specialCooldownCurrent = specialCooldown;

            ProjectileHeroArrow t = (ProjectileHeroArrow)Instantiate(projArrow);
            t.transform.position = transform.position + (Vector3)box.center;
            t.direction = direction;
            t.team = team;
        }
    }
}
