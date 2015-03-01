using UnityEngine;
using System.Collections;

public class ActiveItem : Item
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

    protected override void LateUpdate ()
    {
        if (!held)
        {
            for (i=0; i<players.Length; i++)
            {
                if (players[i].grab)
                {
                    if (collider2D.bounds.Intersects(players[i].collider2D.bounds))
                    {
                        if (players[i].Pickup(this))
                            break;
                    }
                }
            }
        }

    }

    public void Activate ()
    {

    }
}
