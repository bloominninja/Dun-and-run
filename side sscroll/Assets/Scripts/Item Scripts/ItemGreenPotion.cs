using UnityEngine;
using System.Collections;

public class ItemGreenPotion : ActiveItem
{
    public bool on = false;
    private float regenTime = 1.5f;
    private float regenTimeCurrent;
    private int regenCount = 4;
    private int regenCountCurrent;


    protected override void Start ()
    {
        base.Start();
        cooldown = 45;
        cost = 0;
    }

    public override void Tick (PlayerController player)
    {
        base.Tick(player);
        if (on)
        {
            regenTimeCurrent -= Time.deltaTime;
            if (regenTimeCurrent <= 0)
            {
                if (regenCount % 2 == 0)
                {
                    player.currentHealth += 1;
                    if (player.currentHealth > player.maxHealth)
                        player.currentHealth = player.maxHealth;
                }
                else
                {
                    player.currentMagic += 1;
                    if (player.currentMagic > player.maxMagic)
                        player.currentMagic = player.maxMagic;
                }

                regenCountCurrent -= 1;
                if (regenCountCurrent <= 0)
                    on = false;
                else
                    regenTimeCurrent = regenTime;
            }
        }
    }
    
    public override void Activate (PlayerController player)
    {
        on = true;
        regenTimeCurrent = regenTime;
        regenCountCurrent = regenCount;
    }
}
