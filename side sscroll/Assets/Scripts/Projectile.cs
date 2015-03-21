using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    protected BoxCollider2D box;
    public int team;
    public int direction = 1;
    public float life = 0;
    protected PlayerController i;
    
    // Use this for initialization
    protected virtual void Start ()
    {
        box = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    protected virtual void Update ()
    {
        life += Time.deltaTime;
    }
    
    protected virtual void LateUpdate ()
    {
        foreach (PlayerController i in GameManager.o.players)
        {
            if (box.bounds.Intersects(i.box.bounds))
            {
                if (i.team != team)
                if (i.Hit(this))
                    break;
            }
        }
    }
    
    public virtual void Effect (PlayerController player)
    {
        Destroy(this.gameObject);
    }

}
