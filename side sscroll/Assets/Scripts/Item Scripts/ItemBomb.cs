using UnityEngine;
using System.Collections;

public class ItemBomb : ActiveItem
{ 
    public ProjectileBomb projectile, prefab;

    protected override void Start ()
    {
        base.Start();
        cooldown = 7;
        cost = 2;
    }

    public override void Activate (PlayerController player)
    {
        projectile = (ProjectileBomb)Instantiate(prefab);
        projectile.transform.position = player.transform.position + (Vector3)player.box.offset + (Vector3)(box.size / 2);
        projectile.direction = player.direction;
        projectile.team = player.team;
    }
}
