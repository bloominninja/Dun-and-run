using UnityEngine;
using System.Collections;

public class ItemBunnyHood : Item
{

    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        player.jumpHeight += 4;
        player.speed += 2;
        player.physics.gravity += 5;
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.jumpHeight -= 4;
        player.speed -= 2;
        player.physics.gravity -= 5;
    }
}
