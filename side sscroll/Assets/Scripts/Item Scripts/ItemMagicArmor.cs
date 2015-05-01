using UnityEngine;
using System.Collections;

public class ItemMagicArmor : Item
{
    
    public override void OnPickup (PlayerController player)
    {
        base.OnPickup(player);
        player.knockbackMult /= 2;
        player.knockbackDur *= 0.1f;
    }
    
    public override void OnDrop (PlayerController player)
    {
        base.OnDrop(player);
        player.knockbackMult *= 2;
        player.knockbackDur /= 0.1f;
    }
}
