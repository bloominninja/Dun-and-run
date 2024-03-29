﻿using UnityEngine;
using System.Collections;

public class ItemHeartContainer : Item
{

    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        player.maxHealth += 2;
        player.currentHealth += 2;
    }

    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.maxHealth -= 2;
        if (player.currentHealth > player.maxHealth)
            player.currentHealth = player.maxHealth;
    }
}
