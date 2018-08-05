using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : SineWaveProjectile
{
    [Header("Bubble Properties")]
    public float xForce;
    public float yForce;
    public float sizeSpeed = 5f; // Multiplier for sizing interpolation when attached to an enemy
    public float rotationSpeed = 5f; // Mulitplier for rotating the bubble in relation to enemy velocity

    private Enemy enemyAttachedTo;
    private Vector3 bubbleSize;
    private bool isOnTarget;

    private float forceDampening = 2.25f; // Amount used to dampen the force of a bumble impact on flying and already airborne targets

    protected override void Start()
    {
        base.Start();
        CancelInvoke("DestroyProjectile");
    }

    protected override void Update()
    {
        if (isOnTarget)
        {
            if (enemyAttachedTo == null)
            {
                Burst();
                return;
            }

            if (enemyAttachedTo.GetRigidbody2D().velocity.y < 0)
            {
                Burst();
                enemyAttachedTo.TakeDamage(Damage);
                return;
            }

            Vector3 sizeTo = new Vector3(Mathf.Lerp(transform.localScale.x, bubbleSize.x, Time.deltaTime * sizeSpeed),
                Mathf.Lerp(transform.localScale.y, bubbleSize.y, Time.deltaTime * sizeSpeed), 0);

            transform.localScale = sizeTo;

            Vector2 dir = enemyAttachedTo.GetRigidbody2D().velocity;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotateTo = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime * rotationSpeed);

            transform.position = enemyAttachedTo.transform.position;
        }
        else
        {
            base.Update();

            if (GameMaster.GameIsOver)
            {
                Burst();
            }
        }
    }

    void Burst()
    {
        Impact();
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOnTarget)
        {
            enemyAttachedTo = collision.GetComponent<Enemy>();

            if (enemyAttachedTo != null)
            {
                isOnTarget = true;
                enemyAttachedTo.isUnderForces = true;
                enemyAttachedTo.GetRigidbody2D().velocity = Vector3.zero;

                if (enemyAttachedTo.IsAirborne() || enemyAttachedTo.enemyType == Enemy.Type.Flying)
                {
                    enemyAttachedTo.MakeAirborne(xForce, yForce / forceDampening); // Applies less force if enemy is already airborne
                }
                else
                {
                    enemyAttachedTo.MakeAirborne(xForce, yForce);
                }

                Vector3 enemySize = enemyAttachedTo.GetComponent<SpriteRenderer>().bounds.size;

                float enemyLargestSize = enemySize.x < enemySize.y ? enemySize.y : enemySize.x;

                bubbleSize = new Vector3(enemyLargestSize, enemyLargestSize, enemySize.z) * 2.5f;
            }
        }
    }

    void OnBecameInvisible()
    {
        float timeToDestroy = 1f;
        Destroy(gameObject, timeToDestroy);
    }
}
