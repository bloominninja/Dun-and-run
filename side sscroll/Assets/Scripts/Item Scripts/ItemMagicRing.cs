using UnityEngine;
using System.Collections;

public class ItemMagicNecklace : Item
{

    public override void Tick (PlayerController player)
    {
        base.Tick(player);
        player.magicRechargeCurrent -= Time.deltaTime * 0.3f;
    }
}
