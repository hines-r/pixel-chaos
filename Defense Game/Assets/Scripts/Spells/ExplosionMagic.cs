using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionMagic : Attack
{
    public float splashRadius = 5f;

    private readonly float timeAlive = 5f;

    void Start()
    {
        transform.position = Target.transform.position;
        Explode();
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
