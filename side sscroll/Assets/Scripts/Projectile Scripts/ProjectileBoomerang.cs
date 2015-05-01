using UnityEngine;
using System.Collections;

public class ProjectileBoomerang : RangedProjectile
{
    public PlayerController player;
    protected bool turn = false;

    protected override void Start ()
    {
        base.Start();
        damage = 0;
        physics.SetSpeedX(direction * 12, 1);
        physics.SetSpeedY(0, 20);
    }
    
    protected override void Update ()
    {
        if (GameManager.o.pause)
            return;
        life += Time.deltaTime;

        if (!turn && (life > 1 || physics.collide))
        {
            turn = true;
        }
        else if (turn)
        {
            physics.collLayer1 = "null";
            Vector2 l;
            float i;
            l.x = (player.transform.position.x + player.box.size.x / 2) - transform.position.x;
            l.y = (player.transform.position.y + player.box.size.y / 2) - transform.position.y;
            i = Mathf.Sqrt(Mathf.Pow(l.x, 2) + Mathf.Pow(l.y, 2));

            physics.SetSpeedX(l.x * 12 / i, 1);
            physics.SetSpeedY(l.y * 12 / i, 1);
        }
        transform.Rotate(Vector3.forward * 30);
        physics.Move(Vector2.zero);
    }

    protected override void LateUpdate ()
    {
        foreach (PlayerController i in GameManager.o.players)
        {
            if (box.bounds.Intersects(i.box.bounds))
            {
                if (i.team != team || team == 0)
                {
                    if (i.Hit(this))
                        break;
                }
                else if (i.team == team && turn)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public override void Effect (PlayerController player)
    {
        player.Stun(0.7f, direction, getSource());
        turn = true;
    }
}
