﻿using UnityEngine;
using System.Collections;

public class ItemMagicFeather : Item
{

    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        player.extraJumps += 1;
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.extraJumps -= 1;
    }
}
