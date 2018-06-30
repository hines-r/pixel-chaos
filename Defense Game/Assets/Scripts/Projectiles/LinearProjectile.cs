using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile
{
    [Header("Chain")]
    public bool isChaining;
    public float chainingRadius; // Distance in a circle the projectile can chain to

    [Header("Properties")]
    public float speed = 25f;

    private Vector3 velocity;
    private Vector3 previousPosition;

    protected override void Start()
    {
        base.Start();
        FaceTarget();
    }

    protected override void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {
        velocity = (previousPosition - transform.position) / Time.deltaTime;
        previousPosition = transform.position;

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void ChainToRandomEnemy(Enemy justHit)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), chainingRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();

            if (enemy != null && enemy.GetInstanceID() != justHit.GetInstanceID())
            {
                Target = enemy.gameObject;
                FaceTarget();
                return;
            }
        }
    }

    protected virtual void FaceTarget()
    {
        Enemy targetEnemy = Target.GetComponent<Enemy>();
        Vector3 targetPos = targetEnemy.transform.position;

        if (targetEnemy != null && !targetEnemy.isUnderForces)
        {
            targetPos = CalculateFuturePosition(targetEnemy, targetPos);
        }

        transform.right = targetPos - transform.position;
    }

    public Vector3 CalculateFuturePosition(Enemy targetEnemy, Vector3 targetPos)
    {
        Vector3 targetVelocity = targetEnemy.GetVelocity();

        float a = speed * speed - Vector3.Dot(targetVelocity, targetVelocity);
        float b = -2 * Vector3.Dot(targetVelocity, (targetPos - transform.position));
        float c = Vector3.Dot(-(targetPos - transform.position), (targetPos - transform.position));

        // Calculates the total time to the target
        float time = (b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        // Gets the displacement in time of the target
        float displacementX = targetVelocity.x * time;
        float displacementY = targetVelocity.y * time;

        Vector3 displacement = new Vector3(displacementX, displacementY);
        Vector3 futurePosition = targetPos - displacement;

        // Aims at the targets stopping point on the x axis if the displacement exceeds it
        if (targetEnemy.currentState == Enemy.State.Attacking || futurePosition.x < targetEnemy.stoppingPoint)
        {
            targetPos.x = targetEnemy.stoppingPoint;
        }
        else
        {
            targetPos -= displacement;
        }

        return targetPos;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemyHit = collision.GetComponent<Enemy>();

        if (enemyHit != null)
        {
            if (isChaining)
            {
                ChainToRandomEnemy(enemyHit);
            }
        }

        base.OnTriggerEnter2D(collision);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (isChaining)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chainingRadius);
        }
    }
}
