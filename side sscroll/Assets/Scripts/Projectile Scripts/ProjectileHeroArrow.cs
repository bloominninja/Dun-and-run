using UnityEngine;
using System.Collections;

public class ProjectileHeroArrow : RangedProjectile
{
    protected float speed = 8;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        physics.SetSpeedX(speed * direction, 3);
        physics.SetSpeedY(0, 1);
        //transform.rotation = new Quaternion(0, 0, 270, 0);
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

        if (life >= 2)
        {
            Destroy(this.gameObject);
        }
        else if (physics.collide)
        {
            Destroy(this.gameObject);
        }
    }

    protected override void LateUpdate ()
    {
        base.LateUpdate();
    }

    public override void Effect (PlayerController player)
    {
        player.Damage(1, direction);
        Destroy(this.gameObject);
    }
}
