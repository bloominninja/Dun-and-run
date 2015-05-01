using UnityEngine;
using System.Collections;

public class ItemBoots : Item
{
    protected bool onGround = false;

    public override void Tick (PlayerController player)
    {
        base.Tick(player);
        if (player.physics.collideBottom && !onGround)
        {
            onGround = true;
            player.speed += 5;
        }
        else if (!player.physics.collideBottom && onGround)
        {
            onGround = false;
            player.speed -= 5;
        }
    }
}
