using UnityEngine;
using System.Collections;

public class ProjectileHeroBasic : MeleeProjectile
{
    protected override void Start ()
    {
        base.Start();
    }
	
    protected override void Update ()
    {
        base.Update();
    }
    
    public override void Effect (PlayerController player)
    {
        player.Damage(1, direction, getSource());
    }
}
