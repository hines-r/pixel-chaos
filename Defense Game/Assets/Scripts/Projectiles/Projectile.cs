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

    [Header("DOT")]
    public bool hasDot;
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
            Destroy(gameObject);
            return;
        }
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

            if (targetsHit >= penetrationCount)
            {
                if (impactEffect != null)
                {
                    GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
                    Destroy(impact, particleTime);
                }

                Destroy(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, splashRadius);
        }
    }
}
