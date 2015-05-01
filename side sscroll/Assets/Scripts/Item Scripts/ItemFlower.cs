using UnityEngine;
using System.Collections;

public class ItemFlower : Item
{

    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        player.maxMagic += 2;
        player.currentMagic += 2;
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.maxMagic -= 2;
        if (player.currentMagic > player.maxMagic)
            player.currentMagic = player.maxMagic;
    }
}
