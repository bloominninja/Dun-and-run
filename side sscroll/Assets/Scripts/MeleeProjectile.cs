using UnityEngine;
using System.Collections;

public class MeleeProjectile : Projectile
{
    
    public bool on = false;
    protected float onTime;
    
    protected override void Start ()
    {
        base.Start();
    }
    
    protected override void Update ()
    {
        if (GameManager.o.pause)
            return;
        base.Update();
        if (on)
        {
            onTime -= Time.deltaTime;
            if (onTime <= 0)
            {
                on = false;
            }
        }
    }
    
    protected override void LateUpdate ()
    {
        if (on)
            base.LateUpdate();
    }
    
    public virtual void Activate (float time)
    {
        on = true;
        onTime = time;
    }
    
    public virtual void Deactivate ()
    {
        on = false;
    }
}
