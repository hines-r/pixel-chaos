using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    public float speed = 5f;
    public float rotationSpeed = 200f;
    public float splashRadius = 1f;

    public GameObject explosionEffect;
    private float particleTime = 4f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Target != null)
        {
            // Rotates the projectile to face the target upon instantiation
            Vector3 diff = Target.transform.position - transform.position;
            diff.Normalize();

            float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90f);
        }
    }

    void Update()
    {
        if (ProceduralSpawner.EnemiesAlive <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (Target == null)
        {
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        AttackingUnit unit = originEntity.GetComponent<AttackingUnit>();

        if (unit != null)
        {
            Target = unit.ObtainTarget();
        }
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector2 direction = (Vector2)Target.transform.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotationSpeed;
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = transform.up * speed; // Keeps traveling in the foward direction until new target is aquired
        }
    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(explosion, particleTime);

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }

}
