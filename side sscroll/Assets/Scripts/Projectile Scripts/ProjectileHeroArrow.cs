using UnityEngine;
using System.Collections;

public class ProjectileHeroArrow : RangedProjectile
{
    protected float speed = 15;

    private Vector3 t;
    private Quaternion q;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        physics.SetSpeedX(speed * direction, 3);
        physics.SetSpeedY(0, 0.4f);

        t = transform.localScale;
        t.x *= direction * Mathf.Sign(transform.localScale.x);
        transform.localScale = t;
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

        if (life >= 1.2f)
        {
            Destroy(this.gameObject);
        }
        else if (physics.collide)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.Rotate(Vector3.forward * Mathf.Rad2Deg * (Mathf.Atan(physics.speed.y / physics.speed.x) - transform.rotation.z));
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
