using UnityEngine;
using System.Collections;

public class ItemBoomerang : ActiveItem
{ 
    public ProjectileBoomerang projectile, prefab;
    
    protected override void Start ()
    {
        base.Start();
        cooldown = 4;
        cost = 1;
    }

    public override void Activate (PlayerController player)
    {
        projectile = (ProjectileBoomerang)Instantiate(prefab);
        projectile.transform.position = player.transform.position + (Vector3)player.box.offset + (Vector3)(box.size / 2);
        projectile.direction = player.direction;
        projectile.team = player.team;
        projectile.player = player;
    }
}
