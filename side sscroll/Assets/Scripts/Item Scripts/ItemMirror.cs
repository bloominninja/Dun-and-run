using UnityEngine;
using System.Collections;

public class ItemMirror : ActiveItem
{
    
    protected override void Start ()
    {
        base.Start();
        cooldown = 5;
        cost = 1;
    }

    public override void Activate (PlayerController player)
    {
        player.physics.SetSpeedX(300 * player.direction, 0.001f, player.physics.speed.x);
    }
}
