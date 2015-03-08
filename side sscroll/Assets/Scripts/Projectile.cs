using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CustomPhysics))]
public class Projectile : MonoBehaviour
{
    protected CustomPhysics physics;
    protected PlayerController[] players;
    [HideInInspector]
    protected int
        i;

    // Use this for initialization
    void Start ()
    {
        physics = GetComponent<CustomPhysics>();
        players = (PlayerController[])FindObjectsOfType(typeof(PlayerController));
    }
	
    // Update is called once per frame
    void Update ()
    {
        physics.Move(Vector2.zero);
    }
    
    protected virtual void LateUpdate ()
    {
        for (i=0; i<players.Length; i++)
        {
            if (collider2D.bounds.Intersects(players[i].collider2D.bounds))
            {
                if (players[i].Hit(this))
                    break;
            }
        }
    }
    
    public virtual void Effect (PlayerController player)
    {
        Destroy(this.gameObject);
    }
}
