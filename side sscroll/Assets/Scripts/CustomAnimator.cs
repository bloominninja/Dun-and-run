using UnityEngine;
using System.Collections;

public class CustomAnimator : MonoBehaviour
{
    
    public Animator animator;
    protected CustomPhysics physics;
    protected PlayerController player;
    
    public string state;
    
    protected Vector3 t;
    
    public string spriteSheetPath;
    public string whiteSheetPath;
    public Sprite[] spriteSheet;
    public Sprite[] whiteSheet;
    protected SpriteRenderer rend;
    
    protected float opacityTimer = 0.1f;
    protected float whiteTimer = 0.2f;
    protected bool whiteToggle;
    
    // Use this for initialization
    public virtual void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        physics = GetComponent<CustomPhysics>();
        player = GetComponent<PlayerController>();
        
        rend = GetComponentInChildren<SpriteRenderer>();
        
        spriteSheet = Resources.LoadAll<Sprite>(spriteSheetPath);
        whiteSheet = Resources.LoadAll<Sprite>(whiteSheetPath);
    }
    
    // Update is called once per frame
    public virtual void LateUpdate ()
    {
        t = animator.transform.localScale;
        t.x *= player.direction * Mathf.Sign(animator.transform.localScale.x);
        animator.transform.localScale = t;
        if (GameManager.o.pause)
            animator.speed = 0;
        else
            animator.speed = 1;
        
        foreach (Sprite s in spriteSheet)
        {
            if (s.name == rend.sprite.name)
            {
                rend.sprite = s;
                break;
            }
        }
        
        if (!player.invincible || player.defeated || state == "special")
        {
            rend.color = Color.white;
            opacityTimer = 0.1f;
        }
        else
        {
            opacityTimer -= Time.deltaTime;
            if (opacityTimer <= 0)
            {
                if (rend.color == Color.white)
                    rend.color = Color.clear;
                else
                    rend.color = Color.white;
                opacityTimer = 0.1f;
            }
        }
    }
}
