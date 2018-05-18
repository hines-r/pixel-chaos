using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicProjectile : Projectile
{
    public float throwHeight = 5;

    public bool isNearest;
    public bool isRandom;

    private Rigidbody2D rb;

    private float h;
    private float gravity = -9.81f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    void Launch()
    {
        GameObject target = null;

        if (isRandom)
        {
            target = RandomTarget();
        }
        else if (isNearest)
        {
            target = NearestTarget();
        }

        if (target != null)
        {
            rb.velocity = CalculateLaunchVelocity(target);
        }
    }

    void Update()
    {
        if (EnemySpawner.EnemiesAlive <= 0)
        {
            Destroy(gameObject);
            return;
        }

        RotateToTarget();
    }

    Vector2 CalculateLaunchVelocity(GameObject enemyToHit)
    {
        Transform target = enemyToHit.GetComponent<Transform>();
        h = target.position.y - transform.position.y + throwHeight;

        if (h < 0)
        {
            h = 0;
        }

        float targetVelocity = enemyToHit.GetComponent<Enemy>().GetVelocity();

        float displacementX = target.position.x - transform.position.x;
        float displacementY = target.position.y - transform.position.y;

        float time = (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));

        float distanceTraveledInTime = targetVelocity * time;
        float futurePositionX = target.position.x - distanceTraveledInTime;

        if (futurePositionX >= enemyToHit.GetComponent<Enemy>().stoppingPoint)
        {
            displacementX -= distanceTraveledInTime; // Compensates for location of the target moving at a constant speed
        }

        Vector2 velocityX = Vector2.right * displacementX / time;
        Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * gravity * h);

        return velocityX + velocityY * -Mathf.Sign(gravity);
    }

    void RotateToTarget()
    {
        Vector2 velocity = rb.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
