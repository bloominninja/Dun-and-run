using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
public class Item : MonoBehaviour
{
    private CustomPhysics physics;
    // Use this for initialization
    void Start ()
    {
        physics = GetComponent<CustomPhysics>();
    }
	
    // Update is called once per frame
    void Update ()
    {
        physics.Move(new Vector2(0, 0));
    }

    void Pickup ()
    {

    }

    void Drop ()
    {

    }

    void Tick ()
    {

    }
}

public class ActiveItem : Item
{
    void Activate ()
    {
        
    }
}