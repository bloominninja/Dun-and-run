using UnityEngine;
using System.Collections;

public class ProjectileHeroBasic : MeleeProjectile
{
    public Vector3 t1, t2, t3;
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        t1 = box.bounds.center;
        t2 = box.bounds.extents;
    }
    
    public override void Effect (PlayerController player)
    {
        player.Damage(1, direction);
    }
}
