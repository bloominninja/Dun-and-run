using UnityEngine;
using System.Collections;

public class ActiveItem : Item
{
    public float cooldown = 1;
    public int cost = 1;

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

    protected override void LateUpdate ()
    {
        if (!held)
        {
            foreach (PlayerController i in GameManager.o.players)
            {
                if (i.grab)
                {
                    if (box.bounds.Intersects(i.box.bounds))
                    {
                        if (i.Pickup(this))
                            break;
                    }
                }
            }
        }

    }

    public virtual void Activate (PlayerController player)
    {

    }
}
