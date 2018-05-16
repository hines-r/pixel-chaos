using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionMagic : Projectile
{
    public float splashRadius = 5f;

    private Transform target;

    private float timeAlive = 5f;

    void Start()
    {
        target = NearestTarget().transform;
        transform.position = target.position;
        Explode();
    }

    void Update()
    {

    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), splashRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
        }

        Destroy(gameObject, timeAlive);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
