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

    [Header("Spread")]
    public bool hasSpread;
    public float spreadCount;
    [Range(0f, 3f)]
    public float spreadVariance;

    [Space]
    public bool isATrap; // Allows the collider to remain active even if the projectile is on the ground
    private readonly float yOffset = 1f;

    [Header("Bounciness")]
    public bool isBouncy;
    [Range(0f, 1f)]
    public float bounceForce; // Amount of velocity to retain after a bounce
    public int timesToBounce;
    public float targetXOffset; // The position the projectile should land before bouncing
    private float numBounces;
    private bool isBouncing;
    private float finalVelocityY;

    private Rigidbody2D rb;

    private Vector3 impactLocation;
    private Enemy targetEnemy;
    protected bool isOnGround;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    void Launch()
    {
        if (Target != null)
        {
            if (hasSpread)
            {
                for (int i = 0; i < spreadCount; i++)
                {
                    ParabolicProjectile duplicate = Instantiate(this, transform.position, Quaternion.identity);
                    duplicate.Damage = Damage;
                    duplicate.Target = Target;
                    duplicate.spreadCount = 0;
                }
            }

            Vector3 launchVelocity = CalculateLaunchVelocity(Target);

            if (isRotating)
            {
                RotateToTarget(launchVelocity);
            }

            rb.velocity = launchVelocity;
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
                if (CanAttackFlying && targetEnemy != null && targetEnemy.IsAirborne() || targetEnemy.enemyType == Enemy.Type.Flying)
                {
                    Vector3 airborneImpact = targetEnemy.GetAirbornePosition();
                    impactLocation = airborneImpact;
                }

                if (transform.position.x >= impactLocation.x && transform.position.y <= impactLocation.y)
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

        // Makes the bounce perfect by settings the position and velocity to the original impact calculation
        transform.position = impactLocation;
        rb.velocity = new Vector2(rb.velocity.x, finalVelocityY);

        Vector3 projectileVelocity = rb.velocity;
        Vector3 normal = Vector3.up;

        // Gets the reflection of the vector multiplied by the bounce force from 0f - 1f
        Vector3 reflection = bounceForce * (-2 * Vector3.Dot(projectileVelocity, normal) * normal + projectileVelocity);

        rb.velocity = reflection;

        float angle = Vector3.Angle(transform.right, Vector3.right);
        Vector3 initialVelocity = rb.velocity;
        Vector3 finalVelocity = Vector3.zero; // Velocity at max height
        float gravity = Physics2D.gravity.y;

        float time = 2 * (-initialVelocity.y / gravity); // Time at max height multiplied by 2 to get total time to land
        float displacementX = initialVelocity.x * time; // Distance traveled after bouncing
        float displacementY = -(initialVelocity.y * initialVelocity.y) / (2 * gravity); // Max height reached after bounce

        finalVelocityY = initialVelocity.y + gravity * time;

        impactLocation = new Vector3(impactLocation.x + displacementX, impactLocation.y, impactLocation.z);
    }

    Vector3 CalculateLaunchVelocity(GameObject entityToHit)
    {
        targetEnemy = entityToHit.GetComponent<Enemy>();
        Vector3 target = entityToHit.GetComponent<Transform>().position;

        if (hasSpread)
        {
            target.x += Random.Range(-spreadVariance, spreadVariance);
            target.y += Random.Range(-spreadVariance / 2f, spreadVariance / 2f);
        }

        float totalVerticalDisplacement = target.y - transform.position.y + throwHeight; // Peak height of projectile
        float gravity = Physics2D.gravity.y;

        // Won't add any additional throw height if the unit is throwing from a point above the target
        // They will instead simply throw the projectile downwards
        if (totalVerticalDisplacement < 0)
        {
            totalVerticalDisplacement = 0;
        }

        // Will attempt to aim at the stopping point of an enemy if it is attacking
        // This way the projectile won't miss as often when enemies are performing a lunge attack
        if (targetEnemy != null && !hasSpread && targetEnemy.currentState == Enemy.State.Attacking)
        {
            target.x = targetEnemy.stoppingPoint;
        }

        // If the projectile is a trap, only subtracts the offset if the impact location 
        // is greater or equal to the stopping point of the enemy
        // Won't subtract the offset if the enemy is attacking
        // This greatly improves the accuracy of the projectile when an enemy is nearing its attack range
        if (isBouncy)
        {
            if (isATrap && target.x - targetXOffset >= targetEnemy.stoppingPoint && targetEnemy.currentState != Enemy.State.Attacking)
            {
                target.x -= targetXOffset;
            }
            else if (!isATrap && target.x - targetXOffset > transform.position.x)
            {
                target.x -= targetXOffset; // Won't subtract offset if the impact would be behind the unit
            }
        }

        float displacementX = target.x - transform.position.x;
        float displacementY = target.y - transform.position.y;
        float timeToTarget = (Mathf.Sqrt(-2 * totalVerticalDisplacement / gravity) + Mathf.Sqrt(2 * (displacementY - totalVerticalDisplacement) / gravity));

        impactLocation = target; // The calculated impact location unadjusted for future position

        // Calculates future position if the entity to hit is a moving enemy
        // Doesn't need to be calculated for a static enemy like the player
        if (targetEnemy != null && !targetEnemy.isUnderForces)
        {
            float targetVelocity = targetEnemy.GetVelocity().x;
            float distanceTraveledInTime = targetVelocity * timeToTarget;
            float futurePositionX = target.x - distanceTraveledInTime;

            if (futurePositionX >= targetEnemy.stoppingPoint)
            {
                displacementX -= distanceTraveledInTime; // Compensates for location of the target moving at a constant speed

                // The calculated impact location of the projectile adjusted for future position if applicable
                impactLocation = new Vector3(futurePositionX, target.y, target.z);
            }
        }

        Vector3 velocityX = Vector3.right * displacementX / timeToTarget;
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * totalVerticalDisplacement);
        Vector3 initialVelocity = velocityX + velocityY;

        finalVelocityY = initialVelocity.y + gravity * timeToTarget; // The final velocity at impact

        return initialVelocity * -Mathf.Sign(gravity);
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
