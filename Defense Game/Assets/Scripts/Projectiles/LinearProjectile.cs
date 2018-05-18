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

    [Header("Targetting Type (Choose 1)")]
    public bool isNearest;
    public bool isRandom;
    public bool isDotTarget;


    private float timeTillDestroyed = 4f;

    private Transform target;

    void Start()
    {
        UpdateTarget();
        Destroy(gameObject, timeTillDestroyed);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void UpdateTarget()
    {
        if (isRandom)
        {
            if (RandomTarget() != null)
            {
                target = RandomTarget().transform;
                transform.right = target.position - transform.position;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (isDotTarget)
        {
            if (NoDotTarget() != null)
            {
                target = NoDotTarget().transform;
                transform.right = target.position - transform.position;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (isNearest)
        {
            if (NearestTarget() != null)
            {
                target = NearestTarget().transform;
                transform.right = target.position - transform.position;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        targetsHit++;

        if(targetsHit >= penetrationCount)
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
