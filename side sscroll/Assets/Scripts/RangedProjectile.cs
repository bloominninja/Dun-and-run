using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
public class RangedProjectile : Projectile
{
    
    protected CustomPhysics physics;
    
    // Use this for initialization
    protected virtual void Start ()
    {
        base.Start();
        physics = GetComponent<CustomPhysics>();
    }
    
    // Update is called once per frame
    protected virtual void Update ()
    {
        base.Update();
        physics.Move(Vector2.zero);
    }
}
