using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    protected CustomPhysics physics;
    protected BoxCollider2D box;
    public bool held = false;
    protected PlayerController i;

    // Use this for initialization
    protected virtual void Start ()
    {
        physics = GetComponent<CustomPhysics>();
        box = GetComponent<BoxCollider2D>();
    }
	
    // Update is called once per frame
    protected virtual void Update ()
    {
        if (GameManager.o.pause)
            return;
        if (!held)
            physics.Move(Vector2.zero);
    }

    protected virtual void LateUpdate ()
    {
        if (!held)
        {
            foreach (PlayerController i in GameManager.o.players)
            {
                if (box.bounds.Intersects(i.box.bounds))
                {
                    if (i.Pickup(this))
                        break;
                }
            }
        }
    }

    public virtual void OnPickup (PlayerController player)
    {
        held = true;
        GetComponent<SpriteRenderer>().enabled = false;
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
