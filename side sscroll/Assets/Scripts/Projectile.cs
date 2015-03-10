using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    protected PlayerController[] players;
    protected BoxCollider2D box;
    public int team;
    public int direction = 1;
    public float life = 0;
    [HideInInspector]
    protected int
        i;
    
    // Use this for initialization
    protected virtual void Start ()
    {
        players = (PlayerController[])FindObjectsOfType(typeof(PlayerController));
        box = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    protected virtual void Update ()
    {
        life += Time.deltaTime;
    }
    
    protected virtual void LateUpdate ()
    {
        for (i=0; i<players.Length; i++)
        {
            if (box.bounds.Intersects(players[i].box.bounds))
            {
                if (players[i].team != team)
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
