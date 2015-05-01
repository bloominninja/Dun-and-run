using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(CustomAnimator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    //Public Values
    public float speed = 8;
    public Vector2 s;
    public float jumpHeight = 16;
    public int maxHealth = 6;
    public int currentHealth = 6;
    public int maxMagic = 6;
    public int currentMagic = 6;
    public float knockbackMult = 0.7f;
    public float knockbackDur = 0.3f;
    public ActiveItem active1;
    public ActiveItem active2;
    public List<Item> passives;
    public int team = 1;
    public int direction = 1;
    public int extraJumps = 0;
    public int extraJumpsCurrent = 0;
    public float magicRecharge = 5;
    public float magicRechargeCurrent = 0;
    public float basicCooldown = 1;
    public float specialCooldown = 1;
    public float basicCooldownCurrent = 0, specialCooldownCurrent = 0, active1CooldownCurrent = 0, active2CooldownCurrent = 0;
    public bool invincible = false;
    public float invincibleTime = 0;
    public float invincibleDur = 1;
    
    public float specialChargeTime = 0.0f;//moved for AI, should operate the same
    public float specialChargeTimeMax = 0.7f;
    public float basicChargeTime, basicChargeTimeMax = 0.7f;
    
    public bool defeated = false;
    [HideInInspector]
    protected Item
        p;
    [HideInInspector]
    public bool
        grab = false;
    
    //unique number to distinguish which character is in use, set by the prefabs
    //0 is none, and if on an existing character should be treated as an error
    //1 is hero, 2 is princess, 3 is soldier, 4 is hunter
    public int id = 0;
    
    //protected Values
    [HideInInspector]
    public PlayerPhysics
        physics;
    public CustomAnimator animator;
    //[HideInInspector]
    public BoxCollider2D
        box;
    
    protected bool pickup = false;
    protected bool pickupToggle = false;
    protected float wallJump = 0;
    
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
    public bool prevSpecial = false;
    
    protected bool inputLock = false;
    protected float lockTime;
    
    
    //AI specific variables, DO NOT TOUCH -Erin
    public AiBase ai;
    public bool aiEnabled = false;
    public double aiDirection = 0;//-1 left, 0 neutral, 1 right
    public bool aiIdle = false;//will override movement if true
    public bool aiJump = false;//use to proc jump
    public bool aiAttack = false;//use to melee attack
    public bool aiSpecial = false;//use to special attack
    public bool aiItem1 = false;//use to proc item 1
    public bool aiItem2 = false;//use to proc item 2
    public bool aiPickup1 = false;//used to initiate an item pickup
    public bool aiPickup2 = false;//used to initiate an item pickup
    
    protected virtual void Awake ()
    {
        physics = GetComponent<PlayerPhysics>();
        animator = GetComponent<CustomAnimator>();
        box = GetComponent<BoxCollider2D>();
        ai = GetComponent<AiBase>();
        passives = new List<Item>();
    }
    
    protected virtual void Start ()
    {
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
        else if (inputType == "Xcade1")
        {
            horizontal = "Xcade1 Horizontal";
            vertical = "Xcade1 Vertical";
            jump = "Xcade1 Jump";
            attack = "Xcade1 Attack";
            special = "Xcade1 Special";
            item1 = "Xcade1 Item1";
            item2 = "Xcade1 Item2";
            grab1 = "Xcade1 Grab1";
            grab2 = "Xcade1 Grab2";
            pause = "Xcade1 Pause";
            select = "Xcade1 Select";
        }
        else if (inputType == "Xcade2")
        {
            horizontal = "Xcade2 Horizontal";
            vertical = "Xcade2 Vertical";
            jump = "Xcade2 Jump";
            attack = "Xcade2 Attack";
            special = "Xcade2 Special";
            item1 = "Xcade2 Item1";
            item2 = "Xcade2 Item2";
            grab1 = "Xcade2 Grab1";
            grab2 = "Xcade2 Grab2";
            pause = "Xcade2 Pause";
            select = "Xcade2 Select";
        }
        else if (inputType == "AI")
        {
            ai.ourPlayer = this;//this activate the AI, essentially
            aiEnabled = true;
        }
        
        //grab our overhead display
        GetComponent<PlayerOverhead>().connectedPlayer = this;
    }
    
    protected virtual void Update ()
    {
        if (PausePressed())
        {
            GameManager.o.pause = !GameManager.o.pause;
        }
        if (GameManager.o.pause)
            return;
        if (!defeated)
        {
            if (invincible)
            {
                invincibleTime -= Time.deltaTime;
                if (invincibleTime <= 0)
                    invincible = false;
            }
            if (currentMagic < maxMagic)
            {
                magicRechargeCurrent -= Time.deltaTime;
                if (magicRechargeCurrent <= 0)
                {
                    currentMagic += 1;
                    if (currentMagic == maxMagic)
                    {
                        magicRechargeCurrent = magicRecharge;
                    }
                    else
                    {
                        magicRechargeCurrent += magicRecharge;
                    }
                }
            }
            
            if (basicCooldownCurrent > 0)
                basicCooldownCurrent -= Time.deltaTime;
            if (specialCooldownCurrent > 0)
                specialCooldownCurrent -= Time.deltaTime;
            if (active1CooldownCurrent > 0)
                active1CooldownCurrent -= Time.deltaTime;
            if (active2CooldownCurrent > 0)
                active2CooldownCurrent -= Time.deltaTime;
            
            if (wallJump != 0)
            {
                if (Time.deltaTime > Mathf.Abs(wallJump))
                    wallJump = 0;
                else
                    wallJump -= Time.deltaTime * Mathf.Sign(wallJump);
            }
            
            grab = false;            
            pickup = false;
            s = Vector2.zero;
            
            if (inputLock)
            {
                lockTime -= Time.deltaTime;
                if (lockTime <= 0)
                    UnlockInput();
            }
            else
            {
                if (Left())
                {
                    s.x = -speed;
                    direction = -1;
                }
                else if (Right())
                {
                    s.x = speed;
                    direction = 1;
                }
                
                if (Jump())
                {
                    if (physics.speed.y >= 0)
                        s.y = jumpHeight / 1.5f;
                    else
                        s.y = jumpHeight / 5;
                }
                
                if (physics.collideBottom)
                {
                    extraJumpsCurrent = extraJumps;
                    if (JumpPressed())
                    {
                        physics.SetSpeedY(jumpHeight, 0.15f);
                    }
                }
                else if (physics.collideRight)
                {
                    if (Right())
                    {
                        wallJump = 0.1f;
                        if (physics.ledge > 0 && physics.speed.y <= 0)
                        {
                            transform.position = new Vector3(transform.position.x, physics.ledge - transform.localScale.y / 1.5f, transform.position.z);
                            if (JumpPressed())
                            {
                                physics.SetSpeedY(jumpHeight, 0.15f);
                                wallJump = 0;
                            }
                            else
                                physics.SetSpeedY(0, 0);
                        }
                        else if (physics.speed.y <= -physics.gravity / 4)
                        {
                            physics.SetSpeedY(-physics.gravity / 4, 0);
                        }
                    }
                    
                    if (wallJump != 0 && JumpPressed())
                    {
                        physics.SetSpeedX(-speed, 0.15f);
                        physics.SetSpeedY(jumpHeight, 0.15f);
                        wallJump = 0;
                    }
                }
                else if (physics.collideLeft)
                {
                    if (Left())
                    {
                        wallJump = -0.1f;
                        if (physics.ledge > 0 && physics.speed.y <= 0)
                        {
                            transform.position = new Vector3(transform.position.x, physics.ledge - transform.localScale.y / 1.5f, transform.position.z);
                            if (JumpPressed())
                            {
                                physics.SetSpeedY(jumpHeight, 0.15f);
                                wallJump = 0;
                            }
                            else
                                physics.SetSpeedY(0, 0);
                        }
                        else if (physics.speed.y <= -physics.gravity / 4)
                        {
                            physics.SetSpeedY(-physics.gravity / 4, 0);
                        }
                    }
                    
                    if (wallJump != 0 && JumpPressed())
                    {
                        
                        physics.SetSpeedX(speed, 0.15f);
                        physics.SetSpeedY(jumpHeight, 0.15f);
                        wallJump = 0;
                    }
                }
                else if (wallJump != 0 && JumpPressed())
                {
                    if (wallJump > 0)
                    {
                        physics.SetSpeedX(-speed, 0.15f);
                        physics.SetSpeedY(jumpHeight, 0.15f);
                    }
                    else
                    {
                        physics.SetSpeedX(speed, 0.15f);
                        physics.SetSpeedY(jumpHeight, 0.15f);
                    }
                    wallJump = 0;
                }
                else if (extraJumpsCurrent > 0 && JumpPressed())
                {
                    extraJumpsCurrent --;
                    physics.SetSpeedY(jumpHeight, 0.15f);
                }
                else if (extraJumpsCurrent == 0 && JumpPressed() && inputType == "AI")
                {
                    //for scoring the AI for mistakes
                    if (ai != null)
                    {
                        ai.score -= 0.25;
                    }
                }
                
                if (AttackPressed())
                    AttackEffect();
                else if (SpecialPressed())
                    SpecialEffect();
                
                if (Grab1Pressed() || Grab2Pressed() || Down())
                    grab = true;
                
                if (active1 != null && Item1Pressed() && active1CooldownCurrent <= 0 && currentMagic >= active1.cost)
                {
                    active1CooldownCurrent = active1.cooldown;
                    currentMagic -= active1.cost;
                    active1.Activate(this);
                }
                
                if (active2 != null && Item2Pressed() && active2CooldownCurrent <= 0 && currentMagic >= active2.cost)
                {
                    active2CooldownCurrent = active2.cooldown;
                    currentMagic -= active2.cost;
                    active2.Activate(this);
                }
                
            }
            
            foreach (Item p in passives)
            {
                p.Tick(this);
            }
            
            if (active1 != null)
                active1.Tick(this);
            if (active2 != null)
                active2.Tick(this);
        }
        physics.Move(s);
        
        prevLeft = Left();
        prevRight = Right();
        prevUp = Up();
        prevDown = Down();
        if (inputType != "Keyboard")
            prevSpecial = Special();
        
        if (currentHealth <= 0)
        {
            gameObject.layer = 0;
            defeated = true;
            physics.collLayer2 = null;
            animator.state = "defeated";
        }
    }
    
    protected virtual void AttackEffect ()
    {
        if (inputType == "AI")
        {
            if (ai != null)
            {
                ai.score -= 5;//penalize not hitting attacks, gets factored in by the time the attack lands
            }
        }
    }
    
    protected virtual void SpecialEffect ()
    {
        if (inputType == "AI")
        {
            if (ai != null)
            {
                ai.score -= 5;//penalize not hitting attacks, gets factored in by the time the attack lands
            }
        }
    }
    
    public virtual void Bounce (GameObject other, Vector2 sp)
    {
        if (sp.x != 0)
        {
            physics.SetSpeedX(sp.x, 0.2f);
        }
        else if (sp.y != 0)
        {
            if (physics.collideBottom && sp.y < 0)
                physics.SetSpeedX(sp.y * -1 * Mathf.Sign(transform.position.x - other.transform.position.x), 0.2f);
            else
                physics.SetSpeedY(sp.y, 0.2f);
        }
    }
    
    public virtual bool Pickup (Item item)
    {
        if (pickup)
            return false;
        
        passives.Add(item);
        item.OnPickup(this);
        pickup = true;
        return true;
    }
    
    public virtual bool Pickup (ActiveItem item)
    {
        if (pickup)
            return false;
        pickup = true;
        
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
        else if (active1 == null)
        {
            active1 = item;
            item.OnPickup(this);
            return true;
        }
        else if (active2 == null)
        {
            active2 = item;
            item.OnPickup(this);
            return true;
        }
        else if (!pickupToggle)
        {
            Drop(active1);
            active1 = item;
            item.OnPickup(this);
            return true;
        }
        else
        {
            Drop(active2);
            active2 = item;
            item.OnPickup(this);
            return true;
        }
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
    
    public virtual bool Hit (Projectile projectile)
    {
        if (!invincible)
        {
            projectile.Effect(this);
            return true;
        }
        else
            return false;
    }
    
    public virtual void Damage (int damage, int dir, PlayerController source)
    {
        currentHealth -= damage;
        //if (currentHealth <= 0)
        //Destroy(this.gameObject);
        SetInvincible(invincibleDur);
        LockInput(knockbackDur);
        physics.SetSpeedX(speed * knockbackMult * dir, 0.3f);
        physics.SetSpeedY(jumpHeight * knockbackMult, 0);
        direction = dir * -1;
        animator.state = "hurt";
        
        //score depending on source if we are an AI
        if (inputType == "AI")
        {
            if (source != this)
                ai.score += damage * 200;
            else
                ai.score -= damage * 300;
        }
    }
    
    public virtual void Stun (float time, int dir, PlayerController source)
    {
        LockInput(time);
        physics.SetSpeedX(speed * knockbackMult * dir, 0.3f);
        physics.SetSpeedY(jumpHeight * knockbackMult, 0);
        direction = dir * -1;
        animator.state = "hurt";
        
    }
    
    public virtual void LockInput (float time)
    {
        inputLock = true;
        lockTime = time;
    }
    
    public virtual void UnlockInput ()
    {
        inputLock = false;
        animator.state = null;
    }
    
    public virtual void SetInvincible (float time)
    {
        invincible = true;
        invincibleTime = time;
    }
    
    #region Input Functions
    protected virtual bool Left ()
    {
        if (aiEnabled)
        {
            if (aiIdle)
                return false;
            else if (aiDirection < 0)
                return true;
            else
                return false;
        }
        if (Input.GetAxis(horizontal) < 0)
            return true;
        else
            return false;
    }
    
    protected virtual bool LeftPressed ()
    {
        if (aiEnabled)
        {
            if (aiIdle)
                return false;
            else if (prevLeft == false && aiDirection < 0.5)
                return true;
            else
                return false;
        }
        else
        {
            if (prevLeft == false && Left())
                return true;
            else
                return false;
        }
    }
    
    protected virtual bool LeftReleased ()
    {
        if (aiEnabled)
        {
            if (aiIdle)
                return true;
            else if (prevLeft == false || aiDirection < 0)
                return false;
            else
                return true;
        }
        else
        {
            if (prevLeft == true && !Left())
                return true;
            else
                return false;
        }
    }
    
    protected virtual bool Right ()
    {
        if (aiEnabled)
        {
            if (aiIdle)
                return false;
            else if (aiDirection > 0.5)
                return true;
            else
                return false;
        }
        if (Input.GetAxis(horizontal) > 0)
            return true;
        else
            return false;
    }
    
    protected virtual bool RightPressed ()
    {
        if (aiEnabled)
        {
            if (aiIdle)
                return false;
            else if (prevRight == false && aiDirection > 0)
                return true;
            else
                return false;
        }
        else
        {
            if (prevRight == false && Right())
                return true;
            else
                return false;
        }
    }
    
    protected virtual bool RightReleased ()
    {
        if (aiEnabled)
        {
            if (aiIdle)
                return true;
            else if (prevRight == false || aiDirection > 0)
                return false;
            else
                return true;
        }
        else
        {
            if (prevRight == true && !Right())
                return true;
            else
                return false;
        }
    }
    
    protected virtual bool Up ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        if (Input.GetAxis(vertical) > 0)
            return true;
        else
            return false;
    }
    
    protected virtual bool UpPressed ()
    {
        if (prevUp == false && Up())
            return true;
        else
            return false;
    }
    
    protected virtual bool UpReleased ()
    {
        if (prevUp == true && !Up())
            return true;
        else
            return false;
    }
    
    protected virtual bool Down ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        if (Input.GetAxis(vertical) < 0)
            return true;
        else
            return false;
    }
    
    protected virtual bool DownPressed ()
    {
        if (prevDown == false && Down())
            return true;
        else
            return false;
    }
    
    protected virtual bool DownReleased ()
    {
        if (prevDown == true && !Down())
            return true;
        else
            return false;
    }
    
    protected virtual bool Jump ()
    {
        if (aiEnabled)
            return aiJump;
        return Input.GetButton(jump);
    }
    
    protected virtual bool JumpPressed ()
    {
        if (aiEnabled)
            return aiJump;
        else
            return Input.GetButtonDown(jump);
    }
    
    protected virtual bool JumpReleased ()
    {
        if (aiEnabled)
            return !aiJump;
        else
            return Input.GetButtonUp(jump);
    }
    
    protected virtual bool Attack ()
    {
        if (aiEnabled)
            return aiAttack;
        return Input.GetButton(attack);
    }
    
    protected virtual bool AttackPressed ()
    {
        if (aiEnabled)
            return aiAttack;
        else
            return Input.GetButtonDown(attack);
    }
    
    protected virtual bool AttackReleased ()
    {
        if (aiEnabled)
            return !aiAttack;
        else
            return Input.GetButtonUp(attack);
    }
    
    protected virtual bool Special ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        if (inputType == "Keyboard")
            return Input.GetButton(special);
        
        if (Input.GetAxis(special) > 0)
            return true;
        else
            return false;
    }
    
    protected virtual bool SpecialPressed ()
    {
        if (aiEnabled)
            return aiSpecial;
        
        if (inputType == "Keyboard")
            return Input.GetButtonDown(special);
        if (prevSpecial == false && Special())
            return true;
        else
            return false;
    }
    
    protected virtual bool SpecialReleased ()
    {
        if (aiEnabled)
            return !aiSpecial;
        
        if (inputType == "Keyboard")
            return Input.GetButtonUp(special);
        if (prevSpecial == true && !Special())
            return true;
        else
            return false;
    }
    
    protected virtual bool Item1 ()
    {
        if (aiEnabled)
            return aiItem1;
        return Input.GetButton(item1);
    }
    
    protected virtual bool Item1Pressed ()
    {
        if (aiEnabled)
            return aiItem1;
        
        return Input.GetButtonDown(item1);
    }
    
    protected virtual bool Item1Released ()
    {
        if (aiEnabled)
            return !aiItem1;
        
        return Input.GetButtonUp(item1);
    }
    
    protected virtual bool Item2 ()
    {
        if (aiEnabled)
            return aiItem2;
        return Input.GetButton(item2);
    }
    
    protected virtual bool Item2Pressed ()
    {
        if (aiEnabled)
            return aiItem2;
        
        return Input.GetButtonDown(item2);
    }
    
    protected virtual bool Item2Released ()
    {
        if (aiEnabled)
            return !aiItem2;
        
        return Input.GetButtonUp(item2);
    }
    
    protected virtual bool Grab1 ()
    {
        if (aiEnabled)
            return aiPickup1;
        return Input.GetButton(grab1);
    }
    
    protected virtual bool Grab1Pressed ()
    {
        if (aiEnabled)
            return aiPickup1;
        else
            return Input.GetButtonDown(grab1);
    }
    
    protected virtual bool Grab1Released ()
    {
        if (aiEnabled)
            return !aiPickup1;
        else
            return Input.GetButtonUp(grab1);
    }
    
    protected virtual bool Grab2 ()
    {
        if (aiEnabled)
            return aiPickup2;
        return Input.GetButton(grab2);
    }
    
    protected virtual bool Grab2Pressed ()
    {
        if (aiEnabled)
            return aiPickup2;
        else
            return Input.GetButtonDown(grab2);
    }
    
    protected virtual bool Grab2Released ()
    {
        if (aiEnabled)
            return !aiPickup2;
        else
            return Input.GetButtonUp(grab2);
    }
    
    protected virtual bool Pause ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        return Input.GetButton(pause);
    }
    
    protected virtual bool PausePressed ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        return Input.GetButtonDown(pause);
    }
    
    protected virtual bool PauseReleased ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        return Input.GetButtonUp(pause);
    }
    
    protected virtual bool Select ()
    {
        if (aiEnabled)
            return false;//ai can't use this
        return Input.GetButton(select);
    }
    
    protected virtual bool SelectPressed ()
    {
        if (aiEnabled)
            return false;//ai can't use this YET
        return Input.GetButtonDown(select);
    }
    
    protected virtual bool SelectReleased ()
    {
        if (aiEnabled)
            return false;//ai can't use this YET
        return Input.GetButtonUp(select);
    }
    #endregion
}
