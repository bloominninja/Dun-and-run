using UnityEngine;
using System.Collections;

public class HeartContainer : Item
{

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }
	
    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
    }

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
    }
}
