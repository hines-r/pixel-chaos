using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public float speed = 25f;

    [Header("Optional Piercing")]
    public int penetrationCount = 1;
    private int targetsHit;

    [Header("Optional Slow")]
    public float slowAmount = 0;
    public float slowDuration = 0;

    [Header("Optional DOT")]
    public bool hasDot;
    public float damageDuration;

    [Header("Impact Effect (Optional)")]
    public GameObject impactEffect;
    private float particleTime = 3f;

    private float timeTillDestroyed = 4f;

    void Start()
    {
        transform.right = Target.transform.position - transform.position;
        Destroy(gameObject, timeTillDestroyed);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        targetsHit++;

        if (targetsHit >= penetrationCount)
        {
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact, particleTime);
            }

            Destroy(gameObject);
        }

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (hasDot)
            {
                enemy.ApplyDoT(Damage, damageDuration);
            }
            else
            {
                enemy.TakeDamage(Damage);
            }

            if (slowAmount > 0)
            {
                enemy.Slow(slowAmount, slowDuration);
            }
        }
    }

}
