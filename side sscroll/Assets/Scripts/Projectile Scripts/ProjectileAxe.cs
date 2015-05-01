using UnityEngine;
using System.Collections;

public class ProjectileAxe : RangedProjectile
{
    protected override void Start ()
    {
        base.Start();
        physics.SetSpeedX(direction * 4, 20);
        physics.SetSpeedY(18, 0.2f);
        physics.gravity = 15;
        physics.collLayer1 = null;
    }
    
    protected override void Update ()
    {
        if (GameManager.o.pause)
            return;
        base.Update();
        transform.Rotate(Vector3.forward * 15);
        if (transform.position.y < 0)
            Destroy(this.gameObject);
    }
}
