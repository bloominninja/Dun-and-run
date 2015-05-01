using UnityEngine;
using System.Collections;

public class ProjectileLanternFire : RangedProjectile
{

    protected override void Start ()
    {
        base.Start();
        physics.SetSpeedX(direction, 1);
        physics.SetSpeedY(0, 5);
    }
	
    protected override void Update ()
    {
        base.Update();
        if (life < 2)
            physics.SetSpeedX(direction, 1);
        else
            physics.SetSpeedX(0, 1);

        if (life > 5)
            Destroy(this.gameObject);
    }
}
