using UnityEngine;
using System.Collections;

public class ItemLantern : ActiveItem
{
    public ProjectileLanternFire projectile;

    protected override void Start ()
    {
        base.Start();
        cooldown = 6;
        cost = 2;
    }

    public override void Activate (PlayerController player)
    {
        ProjectileLanternFire t = (ProjectileLanternFire)Instantiate(projectile);
        t.transform.position = player.transform.position + (Vector3)box.offset + (Vector3)(box.size / 1.7f);
        t.direction = player.direction;
        t.team = player.team;
    }
}
