using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    [Header("Piercing")]
    public int penetrationCount = 1;
    private int targetsHit;

    [Header("Slow")]
    public float slowAmount = 0;
    public float slowDuration = 0;

    [Header("Stun")]
    public bool hasStun;
    public float stunDuration = 0;

    [Header("DOT")]
    public bool hasDot;
    public bool isStinging; // Instantly applies half the damage if projectile has a DoT
    public float damageDuration;

    [Header("Explosive")]
    public bool isExplosive;
    public float splashRadius;

    [Header("Impact Effect")]
    public GameObject impactEffect;

    private readonly float particleTime = 3f;
    private readonly float maxTimeAlive = 5f;

    protected virtual void Start()
    {
        Destroy(gameObject, maxTimeAlive);
    }

    protected virtual void Update()
    {
        if (ProceduralSpawner.EnemiesAlive <= 0)
        {
            StartCoroutine(DestroyProjectileAtRandomTime());
            return;
        }
    }

    IEnumerator DestroyProjectileAtRandomTime()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        Destroy(gameObject);
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

        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyHit = collision.GetComponent<Enemy>();

        targetsHit++;

        if (enemyHit != null)
        {
            if (hasDot)
            {
                if (isStinging)
                {
                    enemyHit.TakeDamage(Damage / 2);
                }

                enemyHit.ApplyDoT(Damage, damageDuration);
            }
            else
            {
                if (isExplosive)
                {
                    Explode();
                }
                else
                {
                    enemyHit.TakeDamage(Damage);
                }
            }

            if (slowAmount > 0)
            {
                enemyHit.Slow(slowAmount, slowDuration);
            }

            if (hasStun)
            {
                enemyHit.Stun(stunDuration);
            }

            if (targetsHit >= penetrationCount)
            {
                if (impactEffect != null)
                {
                    Impact();
                }

                Destroy(gameObject);
            }
        }
    }

    public void Impact()
    {
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, particleTime);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, splashRadius);
        }
    }
}
