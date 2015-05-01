using UnityEngine;
using System.Collections;

public class ItemAxe : ActiveItem
{
    public ProjectileAxe projectile;
    
    protected override void Start ()
    {
        base.Start();
        cooldown = 4;
        cost = 1;
    }
    
    public override void Activate (PlayerController player)
    {
        ProjectileAxe t = (ProjectileAxe)Instantiate(projectile);
        t.transform.position = player.transform.position + (Vector3)box.offset + (Vector3)(box.size / 2);
        t.direction = player.direction;
        t.team = player.team;
    }
}
