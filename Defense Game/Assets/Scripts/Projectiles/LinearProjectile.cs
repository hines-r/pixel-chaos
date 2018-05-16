using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile
{
    public float speed = 25f;
    public int penetrationCount = 2;
    private int targetsHit;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        targetsHit++;

        if(targetsHit >= penetrationCount)
        {
            Destroy(gameObject);
        }

        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
    }
}
