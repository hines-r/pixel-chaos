using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ParabolicProjectile : Projectile
{
    [Header("Properties")]
    public float throwHeight = 5;
    public bool isRotating = true;
    public bool isCrushing; // The projectile will hit the ground further below its target
    [Space]
    public bool isScatter; // Randomizes launch velocity
    [Range(-2f, 1f)]
    public float minScatter; // Amount of varience when the projectile is set to scatter
    [Range(1f, 2f)]
    public float maxScatter;
    [Space]
    public bool isATrap; // Allows the collider to remain active even if the projectile is on the ground
    private readonly float yOffset = 1f;

    [Header("Bounciness")]
    public bool isBouncy;
    public float timesToBounce;
    [Range(0f, 1f)]
    public float bounceForce;
    public float targetXOffset; // The position the projectile should land before bouncing
    private float numBounces;
    private bool isBouncing;

    private Rigidbody2D rb;

    private Vector3 impactLocation;
    private Enemy targetEnemy;
    protected bool isOnGround;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        if (isRotating)
        {
            RotateToTarget(CalculateLaunchVelocity(Target));
        }

        Launch();
    }

    void Launch()
    {
        if (Target != null)
        {
            rb.velocity = CalculateLaunchVelocity(Target);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (isOnGround)
        {
            return;
        }

        // Simulates the projectile hitting the ground if it never hits a target
        // Only works on friendly projectiles
        if (!isHostile)
        {
            if (isCrushing)
            {
                if (transform.position.x > impactLocation.x && transform.position.y < impactLocation.y - yOffset)
                {
                    HitGround();
                    return;
                }
            }
            else
            {
                if (targetEnemy != null && targetEnemy.IsAirborne())
                {
                    impactLocation = targetEnemy.GetAirbornePosition();
                }

                if (transform.position.x > impactLocation.x && transform.position.y < impactLocation.y)
                {
                    HitGround();
                    return;
                }
            }
        }

        if (isRotating)
        {
            RotateToTarget(rb.velocity);
        }
    }

    void HitGround()
    {
        if (isBouncy)
        {
            if (numBounces < timesToBounce)
            {
                // Only bounces when the velocity is negative
                if (rb.velocity.y < 0f)
                {
                    Bounce();
                    return;
                }
            }
            else
            {
                isBouncing = false;
                isOnGround = true;
            }

            if (isBouncing)
            {
                return;
            }
        }

        // If the projectile has an impact effect (ex. rock), then just 
        // instantiate the effect and destroy the object
        if (impactEffect != null)
        {
            if (isExplosive)
            {
                Explode();
            }

            Impact();
            Destroy(gameObject);
            return;
        }

        // If there is no impact effect, simulates the projectile getting stuck in the ground
        isOnGround = true;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;

        if (!isATrap)
        {
            Collider2D collider = GetComponent<Collider2D>();

            if (collider != null)
            {
                collider.enabled = false;
            }

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Sets the sorting order so enemies appear to walk over the projectile
                spriteRenderer.sortingOrder = -1;
            }
        }
    }

    void Bounce()
    {
        isBouncing = true;
        numBounces++;

        // Gets the reflection of the vector multiplied by the bounce force from 0f-1f
        Vector3 projectileVelocity = rb.velocity;
        Vector3 n = impactLocation - transform.position;
        Vector3 reflection = bounceForce * (-2 * Vector3.Dot(projectileVelocity, n.normalized) * n.normalized + projectileVelocity);

        // Vector should always be positive
        rb.velocity = new Vector3(Mathf.Abs(reflection.x), Mathf.Abs(reflection.y), Mathf.Abs(reflection.z));
    }

    Vector2 CalculateLaunchVelocity(GameObject entityToHit)
    {
        targetEnemy = entityToHit.GetComponent<Enemy>();
        Vector3 target = entityToHit.GetComponent<Transform>().position;

        float h = target.y - transform.position.y + throwHeight;
        float gravity = Physics2D.gravity.y;

        // Won't add any additional throw height if the unit is throwing from a point above the target
        // They will instead simply throw the projectile downwards
        if (h < 0)
        {
            h = 0;
        }

        // Will attempt to aim at the stopping point of an enemy if it is attacking
        // This way the projectile won't miss as often when enemies are performing a lunge attack
        if (targetEnemy != null && targetEnemy.currentState == Enemy.State.Attacking)
        {
            target.x = targetEnemy.stoppingPoint;
        }

        // Offsets the target a bit by the min and max of scatter
        if (isScatter)
        {
            target.x += Random.Range(minScatter, maxScatter);
            target.y += Random.Range(minScatter, maxScatter);
        }

        // Only subtracts the offset if the target position is in front of the origin (facing right)
        if (isBouncy)
        {
            if (transform.position.x - targetXOffset < transform.position.x)
            {
                target.x -= targetXOffset;
            }
        }

        float displacementX = target.x - transform.position.x;
        float displacementY = target.y - transform.position.y;
        float timeToTarget = (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity));

        impactLocation = target; // The calculated impact location unadjusted for future position

        // Calculates future position if the entity to hit is a moving enemy
        // Doesn't need to be calculated for a static enemy like the player
        if (targetEnemy != null && !targetEnemy.isUnderForces)
        {
            float targetVelocity = entityToHit.GetComponent<Enemy>().GetVelocity();
            float distanceTraveledInTime = targetVelocity * timeToTarget;
            float futurePositionX = target.x - distanceTraveledInTime;

            if (futurePositionX >= entityToHit.GetComponent<Enemy>().stoppingPoint)
            {
                displacementX -= distanceTraveledInTime; // Compensates for location of the target moving at a constant speed

                // The calculated impact location of the projectile adjusted for future position if applicable
                impactLocation = new Vector3(futurePositionX, Target.transform.position.y, Target.transform.position.z);
            }
        }

        Vector2 velocityX = Vector2.right * displacementX / timeToTarget;
        Vector2 velocityY = Vector2.up * Mathf.Sqrt(-2 * gravity * h);

        return velocityX + velocityY * -Mathf.Sign(gravity);
    }

    void RotateToTarget(Vector3 velocity)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        Castle castle = collision.GetComponent<Castle>();

        if (castle != null)
        {
            Destroy(gameObject);
            castle.TakeDamage(Damage);
        }
    }
}
