using UnityEngine;
using System.Collections;

public class ItemBluePotion : ActiveItem
{

    protected override void Start ()
    {
        base.Start();
        cooldown = 30;
        cost = 0;
    }

    public override void Activate (PlayerController player)
    {
        player.currentMagic += 3;
        if (player.currentMagic > player.maxMagic)
            player.currentMagic = player.maxMagic;
    }
}
