using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicProjectile : Projectile
{
    public float throwHeight = 5;

    private Rigidbody2D rb;

    private float h;
    private float gravity = -9.81f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        RotateToTarget(CalculateLaunchVelocity(Target));
        Launch();
    }

    void Launch()
    {
        if (Target != null)
        {
            rb.velocity = CalculateLaunchVelocity(Target);
        }
    }

    void Update()
    {
        if (ProceduralSpawner.EnemiesAlive <= 0)
        {
            Destroy(gameObject);
            return;
        }

        RotateToTarget(rb.velocity);
    }

    Vector2 CalculateLaunchVelocity(GameObject entityToHit)
    {
        Enemy enemy = entityToHit.GetComponent<Enemy>();

        Vector3 target = entityToHit.GetComponent<Transform>().position;
        h = target.y - transform.position.y + throwHeight;

        // Won't add any additional throw height if the unit is throwing from a point above the target
        // They will instead simply throw the projectile downwards
        if (h < 0)
        {
            h = 0;
        }

        // Will attempt to aim at the stopping point of an enemy if it is attacking
        // This way the projectile won't miss as often when enemies are performing a lunge attack
        if (enemy != null && enemy.currentState == Enemy.State.Attacking)
        {
            target.x = enemy.stoppingPoint;
        }

        float displacementX = target.x - transform.position.x;
        float displacementY = target.y - transform.position.y;

        float time = (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));

        // Calculates future position if the entity to hit is a moving enemy
        // Doesn't need to be calculated for a static enemy like the player
        if (enemy != null && !enemy.isUnderForces)
        {
            float targetVelocity = entityToHit.GetComponent<Enemy>().GetVelocity();
            float distanceTraveledInTime = targetVelocity * time;
            float futurePositionX = target.x - distanceTraveledInTime;

            if (futurePositionX >= entityToHit.GetComponent<Enemy>().stoppingPoint)
            {
                displacementX -= distanceTraveledInTime; // Compensates for location of the target moving at a constant speed
            }
        }

        Vector2 velocityX = Vector2.right * displacementX / time;
        Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * gravity * h);

        return velocityX + velocityY * -Mathf.Sign(gravity);
    }

    void RotateToTarget(Vector3 velocity)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            Destroy(gameObject);
            enemy.TakeDamage(Damage);
            return;
        }

        Castle castle = collision.GetComponent<Castle>();

        if (castle != null)
        {
            Destroy(gameObject);
            castle.TakeDamage(Damage);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
