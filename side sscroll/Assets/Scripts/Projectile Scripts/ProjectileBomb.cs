using UnityEngine;
using System.Collections;

public class ProjectileBomb : MeleeProjectile
{
    public ProjectileBombExplosion projectile, prefab;

    protected override void Update ()
    {
        if (GameManager.o.pause)
            return;
        base.Update();
        if (life >= 2)
        {
            projectile = (ProjectileBombExplosion)Instantiate(prefab);
            projectile.transform.position = transform.position;
            projectile.direction = direction;
            projectile.team = team;
            Destroy(this.gameObject);

        }
        
    }
    
    protected override void LateUpdate ()
    {
        return;
    }
    
    public override void Effect (PlayerController player)
    {
        return;
    }
}
