using UnityEngine;
using System.Collections;

public class ItemTurkey : Item
{
    protected float healTimer;
    protected float healDelay = 20;

    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        healTimer = healDelay;
    }

    public override void Tick (PlayerController player)
    {
        base.Tick(player);
        healTimer -= Time.deltaTime;
        if (healTimer <= 0)
        {
            healTimer = healDelay;
            player.currentHealth += 1;
            if (player.currentHealth > player.maxHealth)
                player.currentHealth = player.maxHealth;
        }
    }
}
