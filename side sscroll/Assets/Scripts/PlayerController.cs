using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CustomPhysics))]
[RequireComponent(typeof(CustomAnimator))]
public class PlayerController : MonoBehaviour
{
    //Public Values
    public float speed = 8;
    public Vector2 s;
    public float jumpHeight = 12;
    public int maxHealth = 6;
    public int currentHealth = 6;
    public int maxMagic = 100;
    public int currentMagic = 100;
    public ActiveItem active1;
    public ActiveItem active2;
    public LinkedList<Item> passives;
    [HideInInspector]
    protected LinkedListNode<Item>
        p;

    //protected Values
    protected CustomPhysics physics;

    //input-focused variables
    public string inputType = "Keyboard";
    //input string variables, use any of the strings in the Input Manager
    //can be changed on the fly to rebind keys and set up control devices
    protected string horizontal;
    protected string vertical;
    protected string jump;
    protected string attack;
    protected string special;
    protected string item1;
    protected string item2;
    protected string grab1;
    protected string grab2;
    protected string pause;
    protected string select;
    //axis-focused variables; used for pressed and released functions
    protected bool prevLeft = false;
    protected bool prevRight = false;
    protected bool prevUp = false;
    protected bool prevDown = false;
    protected bool prevSpecial = false;
    [HideInInspector]
    public bool
        grab = false;

    void Start ()
    {
        physics = GetComponent<CustomPhysics>();
        passives = new LinkedList<Item>();

        if (inputType == "Keyboard")
        {
            horizontal = "KB Horizontal";
            vertical = "KB Vertical";
            jump = "KB Jump";
            attack = "KB Attack";
            special = "KB Special";
            item1 = "KB Item1";
            item2 = "KB Item2";
            grab1 = "KB Grab1";
            grab2 = "KB Grab2";
            pause = "KB Pause";
            select = "KB Select";
        }
        else if (inputType == "Joy1")
        {
            horizontal = "Joy1 Horizontal";
            vertical = "Joy1 Vertical";
            jump = "Joy1 Jump";
            attack = "Joy1 Attack";
            special = "Joy1 Special";
            item1 = "Joy1 Item1";
            item2 = "Joy1 Item2";
            grab1 = "Joy1 Grab1";
            grab2 = "Joy1 Grab2";
            pause = "Joy1 Pause";
            select = "Joy1 Select";
        }
        else if (inputType == "Joy2")
        {
            horizontal = "Joy2 Horizontal";
            vertical = "Joy2 Vertical";
            jump = "Joy2 Jump";
            attack = "Joy2 Attack";
            special = "Joy2 Special";
            item1 = "Joy2 Item1";
            item2 = "Joy2 Item2";
            grab1 = "Joy2 Grab1";
            grab2 = "Joy2 Grab2";
            pause = "Joy2 Pause";
            select = "Joy2 Select";
        }
        else if (inputType == "Joy3")
        {
            horizontal = "Joy3 Horizontal";
            vertical = "Joy3 Vertical";
            jump = "Joy3 Jump";
            attack = "Joy3 Attack";
            special = "Joy3 Special";
            item1 = "Joy3 Item1";
            item2 = "Joy3 Item2";
            grab1 = "Joy3 Grab1";
            grab2 = "Joy3 Grab2";
            pause = "Joy3 Pause";
            select = "Joy3 Select";
        }
        else if (inputType == "Joy4")
        {
            horizontal = "Joy4 Horizontal";
            vertical = "Joy4 Vertical";
            jump = "Joy4 Jump";
            attack = "Joy4 Attack";
            special = "Joy4 Special";
            item1 = "Joy4 Item1";
            item2 = "Joy4 Item2";
            grab1 = "Joy4 Grab1";
            grab2 = "Joy4 Grab2";
            pause = "Joy4 Pause";
            select = "Joy4 Select";
        }
    }
    
    void Update ()
    {
        if (Left())
            s.x = -speed;
        else if (Right())
            s.x = speed;
        else
            s.x = 0;

        if (Jump())
        {
            s.y = jumpHeight / 2;
        }
        else
        {
            s.y = 0;
        }

        if (physics.collideBottom)
        {
            if (JumpPressed())
                physics.SetSpeedY(jumpHeight, 0.15f);
        }
        else if (physics.collideRight)
        {
            if (JumpPressed())
            {
                physics.SetSpeedX(-speed, 0.15f);
                physics.SetSpeedY(jumpHeight, 0.15f);
            }
        }
        else if (physics.collideLeft)
        {
            if (JumpPressed())
            {
                physics.SetSpeedX(speed, 0.15f);
                physics.SetSpeedY(jumpHeight, 0.15f);
            }
        }

        grab = false;
        if (Grab1Pressed() || Grab2Pressed())
            grab = true;

        p = passives.First;
        while (p!=null)
        {
            p.Value.Tick(this);
            p = p.Next;
        }

        if (active1 != null)
            active1.Tick(this);
        if (active2 != null)
            active2.Tick(this);


        physics.Move(s);


        /*
        if (Left())
            targetSpeed = -speed;
        else if (Right())
            targetSpeed = speed;
        else
            targetSpeed = 0;
        currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

        if (playerPhysics.grounded)
        {
            amountToMove.y = 0;
            RJumps = jumps;
            wallJump = false;
        }
        
        if (playerPhysics.stopMove)
        {
            if (currentSpeed > 0)
            {
                right = true;
                Debug.Log("right");
            }
            else if (currentSpeed < 0)
            {
                right = false;
                Debug.Log("left");
            }
            if (!playerPhysics.grounded)
            {
                wallJump = true;
            }
            targetSpeed = 0;
            currentSpeed = 0;

        }
        if (JumpPressed())
        {

            if (wallJump)
            {
                if (right)
                {
                    currentSpeed = -18;
                    right = false;
                }
                else
                {
                    currentSpeed = 18;
                    right = true;
                }
                amountToMove.y = jumpHeight;

            }
            else if ((RJumps > 0))
            {
                amountToMove.y = jumpHeight;
                RJumps -= 1;
            }
            wallJump = false;
        }

        amountToMove.x = currentSpeed;
        amountToMove.y -= gravity * Time.deltaTime;
        playerPhysics.Move(amountToMove * Time.deltaTime);
        */
        prevLeft = Left();
        prevRight = Right();
        prevUp = Up();
        prevDown = Down();
        if (inputType != "Keyboard")
            prevSpecial = Special();
    }

    /*
    protected float IncrementTowards (float n, float target, float a)
    {
        if (n == target)
        {
            return n;
        }
        else
        {
            float dir = Mathf.Sign(target - n);
            n += a * Time.deltaTime * dir;
            
            if (dir == Mathf.Sign(target - n))
            {
                return n;
            }
            else
            {
                return target;
            }
        }
    }*/

    public virtual void Bounce (GameObject other, Vector2 sp)
    {
        if (sp.x != 0)
        {
            physics.SetSpeedX(sp.x, 0.2f);
        }
        else if (sp.y != 0)
            physics.SetSpeedY(sp.y, 0.2f);
    }
    
    public virtual bool Pickup (Item item)
    {
        passives.AddLast(item);
        item.OnPickup(this);
        return true;
    }

    public virtual bool Pickup (ActiveItem item)
    {
        if (Grab1Pressed())
        {
            if (active1 != null)
                Drop(active1);
            active1 = item;
            item.OnPickup(this);
            return true;
        }
        else if (Grab2Pressed())
        {
            if (active2 != null)
                Drop(active2);
            active2 = item;
            item.OnPickup(this);
            return true;
        }
        return false;
    }
    
    public virtual bool Drop (Item item)
    {
        item.OnDrop(this);
        passives.Remove(item);
        return true;
    }

    public virtual bool Drop (ActiveItem item)
    {
        if (active1 == item)
        {
            item.OnDrop(this);
            active1 = null;
            return true;
        }
        else if (active2 == item)
        {
            item.OnDrop(this);
            active2 = null;
            return true;
        }
        return false;
    }

    #region Input Functions
    protected bool Left ()
    {
        if (Input.GetAxis(horizontal) < 0)
            return true;
        else
            return false;
    }

    protected bool LeftPressed ()
    {
        if (prevLeft == false && Left())
            return true;
        else
            return false;
    }

    protected bool LeftReleased ()
    {
        if (prevLeft == true && !Left())
            return true;
        else
            return false;
    }
    
    protected bool Right ()
    {
        if (Input.GetAxis(horizontal) > 0)
            return true;
        else
            return false;
    }

    protected bool RightPressed ()
    {
        if (prevRight == false && Right())
            return true;
        else
            return false;
    }

    protected bool RightReleased ()
    {
        if (prevRight == true && !Right())
            return true;
        else
            return false;
    }
    
    protected bool Up ()
    {
        if (Input.GetAxis(vertical) > 0)
            return true;
        else
            return false;
    }

    protected bool UpPressed ()
    {
        if (prevUp == false && Up())
            return true;
        else
            return false;
    }

    protected bool UpReleased ()
    {
        if (prevUp == true && !Up())
            return true;
        else
            return false;
    }
    
    protected bool Down ()
    {
        if (Input.GetAxis(vertical) < 0)
            return true;
        else
            return false;
    }

    protected bool DownPressed ()
    {
        if (prevDown == false && Down())
            return true;
        else
            return false;
    }

    protected bool DownReleased ()
    {
        if (prevDown == true && !Down())
            return true;
        else
            return false;
    }
    
    protected bool Jump ()
    {
        return Input.GetButton(jump);
    }
    
    protected bool JumpPressed ()
    {
        return Input.GetButtonDown(jump);
    }
    
    protected bool JumpReleased ()
    {
        return Input.GetButtonUp(jump);
    }

    protected bool Attack ()
    {
        return Input.GetButton(attack);
    }
    
    protected bool AttackPressed ()
    {
        return Input.GetButtonDown(attack);
    }
    
    protected bool AttackReleased ()
    {
        return Input.GetButtonUp(attack);
    }

    protected bool Special ()
    {
        if (inputType == "Keyboard")
            return Input.GetButton(special);

        if (Input.GetAxis(special) > 0)
            return true;
        else
            return false;
    }
    
    protected bool SpecialPressed ()
    {
        if (inputType == "Keyboard")
            return Input.GetButtonDown(special);
        if (prevSpecial == false && Special())
            return true;
        else
            return false;
    }
    
    protected bool SpecialReleased ()
    {
        if (inputType == "Keyboard")
            return Input.GetButtonUp(special);
        if (prevSpecial == true && !Special())
            return true;
        else
            return false;
    }

    protected bool Item1 ()
    {
        return Input.GetButton(item1);
    }
    
    protected bool Item1Pressed ()
    {
        return Input.GetButtonDown(item1);
    }
    
    protected bool Item1Released ()
    {
        return Input.GetButtonUp(item1);
    }

    protected bool Item2 ()
    {
        return Input.GetButton(item2);
    }
    
    protected bool Item2Pressed ()
    {
        return Input.GetButtonDown(item2);
    }
    
    protected bool Item2Released ()
    {
        return Input.GetButtonUp(item2);
    }

    protected bool Grab1 ()
    {
        return Input.GetButton(grab1);
    }
    
    protected bool Grab1Pressed ()
    {
        return Input.GetButtonDown(grab1);
    }
    
    protected bool Grab1Released ()
    {
        return Input.GetButtonUp(grab1);
    }

    protected bool Grab2 ()
    {
        return Input.GetButton(grab2);
    }
    
    protected bool Grab2Pressed ()
    {
        return Input.GetButtonDown(grab2);
    }
    
    protected bool Grab2Released ()
    {
        return Input.GetButtonUp(grab2);
    }

    protected bool Pause ()
    {
        return Input.GetButton(pause);
    }
    
    protected bool PausePressed ()
    {
        return Input.GetButtonDown(pause);
    }
    
    protected bool PauseReleased ()
    {
        return Input.GetButtonUp(pause);
    }

    protected bool Select ()
    {
        return Input.GetButton(select);
    }
    
    protected bool SelectPressed ()
    {
        return Input.GetButtonDown(select);
    }
    
    protected bool SelectReleased ()
    {
        return Input.GetButtonUp(select);
    }
    #endregion
}
