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
    [HideInInspector]
    protected BoxCollider2D
        box;

    // Use this for initialization
    protected virtual void Start ()
    {
        physics = GetComponent<CustomPhysics>();
        players = (PlayerController[])FindObjectsOfType(typeof(PlayerController));
        box = GetComponent<BoxCollider2D>();
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
                if (collider2D.bounds.Intersects(players[i].collider2D.bounds))
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
        renderer.enabled = false;
    }

    public virtual void OnDrop (PlayerController player)
    {
        held = false;
        renderer.enabled = true;
        transform.position = player.transform.position;
    }

    public virtual void Tick (PlayerController player)
    {
        //Debug.Log("hi there, I'm an item.");
    }
}
