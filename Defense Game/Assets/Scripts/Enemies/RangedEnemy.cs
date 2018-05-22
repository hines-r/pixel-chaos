using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Projectile")]
    public GameObject projectile;

    [Header("Target")]
    public GameObject castle;

    protected override IEnumerator Attack()
    {
        currentState = State.Attacking;

        GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity);
        Projectile projectileToFire = obj.GetComponent<Projectile>();

        if (projectileToFire != null)
        {
            projectileToFire.originEntity = gameObject;
            projectileToFire.Damage = damage;
            projectileToFire.Target = castle;
        }

        yield return null;
    }

}
