using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Projectile")]
    public GameObject projectile;

    private TargetingEntity targetingEntity;

    protected override void Start()
    {
        base.Start();
        targetingEntity = GetComponent<TargetingEntity>();
    }

    protected override IEnumerator Attack()
    {
        GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity);
        Projectile projectileToFire = obj.GetComponent<Projectile>();

        if (projectileToFire != null)
        {
            projectileToFire.originEntity = gameObject;
            projectileToFire.Damage = damage;
            projectileToFire.Target = targetingEntity.TargetPlayer().transform;
        }

        yield return null;
    }

}
