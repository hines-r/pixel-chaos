using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Projectile")]
    public GameObject projectile;

    [Header("Target")]
    public GameObject castle;

    private Vector3 target;

    protected override void Start()
    {
        base.Start();

        target = castle.transform.position;
    }

    protected override IEnumerator Attack()
    {
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
