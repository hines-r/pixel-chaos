using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Projectile
{
    public float duration;
    public float damageTickTime; // How often to damage enemies within the radius

    private CircleCollider2D c2d;

    void Start()
    {
        c2d = GetComponent<CircleCollider2D>();
        transform.position = Target.transform.position;

        StartCoroutine(DamageEnemiesWithin());
    }

    void Update()
    {   
        if (duration < 0)
        {
            SelfDestruct();
        }

        duration -= Time.deltaTime;
    }

    void SelfDestruct()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), c2d.radius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.isUnderForces = false;
            }
        }

        Destroy(gameObject);
    }

    IEnumerator DamageEnemiesWithin()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageTickTime);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), c2d.radius);

            foreach (Collider2D nearbyObject in colliders)
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.isUnderForces = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.isUnderForces = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, c2d.radius);
    }
}
