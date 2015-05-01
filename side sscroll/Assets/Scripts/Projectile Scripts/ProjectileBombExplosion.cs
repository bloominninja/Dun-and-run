using UnityEngine;
using System.Collections;

public class ProjectileBombExplosion : MeleeProjectile
{
    
    protected override void Start ()
    {
        base.Start();
        damage = 2;
        Activate(0.5f);
    }
    
    protected override void Update ()
    {
        if (GameManager.o.pause)
            return;
        if (on)
        {
            onTime -= Time.deltaTime;
            if (onTime <= 0)
            {
                on = false;
                Destroy(this.gameObject);
            }
        }
    }
}
