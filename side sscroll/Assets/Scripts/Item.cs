using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
public class Item : MonoBehaviour
{
    protected CustomPhysics physics;
    protected PlayerController[] players;
    protected bool held = false;
    [HideInInspector]
    protected int
        i;

    // Use this for initialization
    protected virtual void Start ()
    {
        physics = GetComponent<CustomPhysics>();
        players = (PlayerController[])FindObjectsOfType(typeof(PlayerController));
    }
	
    // Update is called once per frame
    protected virtual void Update ()
    {
        if (!held)
            physics.Move(Vector2.zero);
    }

    protected virtual void LateUpdate ()
    {
        if (!held)
        {
            for (i=0; i<players.Length; i++)
            {
                if (GetComponent<Collider2D>().bounds.Intersects(players[i].GetComponent<Collider2D>().bounds))
                {
                    if (players[i].Pickup(this))
                        break;
                }
            }
        }
    }

    public virtual void OnPickup (PlayerController player)
    {
        held = true;
        GetComponent<Renderer>().enabled = false;
    }

    public virtual void OnDrop (PlayerController player)
    {
        held = false;
        GetComponent<Renderer>().enabled = true;
        transform.position = player.transform.position;
    }

    public virtual void Tick (PlayerController player)
    {
        //Debug.Log("hi there, I'm an item.");
    }
}
