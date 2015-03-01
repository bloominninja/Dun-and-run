using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CustomPhysics : MonoBehaviour
{
    //public LayerMask collisionMask = 8;
    protected BoxCollider2D box;
    protected Vector2 siz;
    protected Vector2 cen;
    //NEVER CHANGE BELOW LINE OR UNITY COMPILE FUNNY
    protected float skin = 0.005f;
    public Vector2 speed;
    public float gravity = 20;
    public bool gravEnabled = true;
    public float friction = 5;
    public bool lockX;
    public bool lockY;
    protected float lockXTime;
    protected float lockYTime;
    public bool collideRight, collideLeft, collideTop, collideBottom;
    public string collLayer1 = "Terrain";
    public string collLayer2;
    public string collLayer3;
    [HideInInspector]
    protected bool
        collide;
    [HideInInspector]
    protected Ray2D
        ray;
    [HideInInspector]
    protected RaycastHit2D[]
        hits;
    [HideInInspector]
    protected Vector2
        o, d, sp;
    [HideInInspector]
    protected int
        i, i2, originalLayer;

    protected virtual void Start ()
    {
        box = GetComponent<BoxCollider2D>();
        siz = box.size;
        cen = box.center;
    }

    protected virtual void Update ()
    {
        if (lockX)
        {
            lockXTime -= Time.deltaTime;
            if (lockXTime <= 0)
                lockX = false;
        }
        if (lockY)
        {
            lockYTime -= Time.deltaTime;
            if (lockYTime <= 0)
                lockY = false;
        }
    }

    public void Move (Vector2 target)
    {
        if (!lockX)
        {
            if (collideBottom && ((target.x == 0 && Mathf.Abs(speed.x) > 1) || (target.x != 0 && Mathf.Sign(speed.x) != Mathf.Sign(target.x))))
                speed.x = Accelerate(speed.x, target.x - friction * Mathf.Sign(speed.x));
            else
                speed.x = Accelerate(speed.x, target.x);
        }
        if (!lockY)
        {
            if (gravEnabled)
                speed.y = Accelerate(speed.y, target.y - gravity);
            else
                speed.y = Accelerate(speed.y, target.y);
        }
        sp = speed * Time.deltaTime;
        
        collide = false;
        collideRight = false;
        collideLeft = false;
        collideTop = false;
        collideBottom = false;

        originalLayer = gameObject.layer;
        gameObject.layer = 2;
        #region Vertical Collisions
        //check for vertical collision
        if (sp.y != 0)
        {
            for (i=0; i<3; i++)
            {
                d.Set(0, Mathf.Sign(sp.y));
                o.Set((transform.position.x + cen.x - siz.x / 2) + siz.x / 2 * i, transform.position.y + cen.y + siz.y / 2 * d.y);
                ray = new Ray2D(o, d);
                Debug.DrawRay(ray.origin, ray.direction);
                hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Abs(sp.y) + skin, LayerMask.GetMask(collLayer1, collLayer2, collLayer3));
                if (hits.Length != 0)
                {
                    if (CollideV())
                        break;
                }
            }
        }
        #endregion

        #region Horizontal Collisions
        //check for horizontal collision
        if (sp.x != 0)
        {
            for (i=0; i<3; i++)
            {
                d.Set(Mathf.Sign(sp.x), 0);
                o.Set(transform.position.x + cen.x + siz.x / 2 * d.x, (transform.position.y + cen.y - siz.y / 2) + siz.y / 2 * i);
                ray = new Ray2D(o, d);
                Debug.DrawRay(ray.origin, ray.direction);
                hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Abs(sp.x) + skin, LayerMask.GetMask(collLayer1, collLayer2, collLayer3));
                if (hits.Length != 0)
                {
                    if (CollideH())
                        break;
                }
            }
        }
        #endregion

        #region Digonal Collision
        //check for diagonal collisions
        if (!collide && sp.x != 0 && sp.y != 0)
        {
            o.Set(transform.position.x + cen.x + siz.x / 2 * Mathf.Sign(sp.x), transform.position.y + cen.y + siz.y / 2 * Mathf.Sign(sp.y));
            d.Set(sp.normalized.x, sp.normalized.y);
            ray = new Ray2D(o, d);
            Debug.DrawRay(ray.origin, ray.direction);
            hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Sqrt(Mathf.Pow(sp.x, 2) + Mathf.Pow(sp.y, 2)), LayerMask.GetMask(collLayer1, collLayer2, collLayer3));
            if (hits.Length != 0)
            {
                CollideD();
            }
        }
        #endregion
        gameObject.layer = originalLayer;

        transform.Translate(sp);



        /*
        float deltaY = sp.y;
        float deltaX = sp.x;
        //Vector2 p = transform.position;
        grounded = false;


        //Check Bot/Top detection
        for (int i=0; i<3; i++)
        {
            float dir = Mathf.Sign(deltaY);
            float x = (p.x + c.x - s.x / 2) + s.x / 2 * i;
            float y = p.y + c.y + s.y / 2 * dir;

            ray = new Ray(new Vector2(x, y), new Vector2(0, dir));
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask))
            {
                float dst = Vector3.Distance(ray.origin, hit.point);
                if (dst > skin)
                {
                    deltaY = dst * dir - skin * dir;
                }
                else
                {
                    deltaY = 0;

                }
                grounded = true;
                break;
            }
        }
    
        //Check Left/Right
        stopMove = false;
        touchWall = false;
        for (int i=0; i<3; i++)
        {
            float dir = Mathf.Sign(deltaX);
            float x = p.x + c.x + s.x / 2 * dir;
            float y = p.y + c.y - s.y / 2 + s.y / 2 * i;
            
            ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask))
            {
                float dst = Vector3.Distance(ray.origin, hit.point);
                if (dst > skin)
                {
                    deltaX = dst * dir - skin * dir;

                }
                else
                {
                    deltaX = 0;

                    
                }
                stopMove = true;
                touchWall = true;

                //grounded=true;
                break;
            }
        }

        Vector3 playerDir = new Vector3(deltaX, deltaY);
        Vector3 o = new Vector3(p.x + c.x + s.x / 2 * Mathf.Sign(deltaX), p.y + c.y + s.y / 2 * Mathf.Sign(deltaY));
        ray = new Ray(o, playerDir.normalized);
        if (Physics.Raycast(ray, Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY)), collisionMask))
        {
            grounded = true;
            deltaY = 0;
        }

        Vector2 finalTransform = new Vector2(deltaX, deltaY);
        transform.Translate(finalTransform);*/
    }

    protected float Accelerate (float current, float target)
    {
        float accel = Mathf.Max(10, 3 * Mathf.Abs(target - current));
        if (current == target)
        {
            return current;
        }
        else
        {
            float dir = Mathf.Sign(target - current);
            current += accel * Time.deltaTime * dir;
            
            if (dir == Mathf.Sign(target - current))
            {
                return current;
            }
            else
            {
                return target;
            }
        }
    }

    protected virtual bool CollideH ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            collide = true;
            if (ray.direction.x == 1)
                collideRight = true;
            else
                collideLeft = true;
            lockX = false;
        
            float dst = Vector2.Distance(ray.origin, hits[i2].point);
            if (dst > skin)
            {
                speed.x = 0;
                sp.x = dst * ray.direction.x - skin * ray.direction.x;
            }
            else
            {
                speed.x = 0;
                sp.x = 0;
            }
            return true;
        }
        return false;
    }

    protected virtual bool CollideV ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            collide = true;
            if (ray.direction.y == 1)
                collideTop = true;
            else
                collideBottom = true;
            lockY = false;
        
            float dst = Vector3.Distance(ray.origin, hits[i2].point);
            if (dst > skin)
            {
                speed.y = 0;
                sp.y = dst * ray.direction.y - skin * ray.direction.y;
            }
            else
            {
                speed.y = 0;
                sp.y = 0; 
            }
            return true;
        }
        return false;
    }

    protected virtual bool CollideD ()
    {
        for (i2=0; i2<hits.Length; i2++)
        {
            collideBottom = true;
            speed.y = 0;
            sp.y = 0;
            return true;
        }
        return false;
    }
    
    public void SetSpeedY (float target, float time)
    {
        speed.y = target;
        lockY = true;
        lockYTime = time;
    }

    public void SetSpeedX (float target, float time)
    {
        speed.x = target;
        lockX = true;
        lockXTime = time;
    }
}
