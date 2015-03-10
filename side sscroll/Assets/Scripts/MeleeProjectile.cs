using UnityEngine;
using System.Collections;

public class MeleeProjectile : Projectile
{

    public bool on = false;

    // Use this for initialization
    protected virtual void Start ()
    {
        base.Start();
    }
    
    // Update is called once per frame
    protected virtual void Update ()
    {

    }

    protected virtual void LateUpdate ()
    {
        if (on)
            base.LateUpdate();
    }
}
