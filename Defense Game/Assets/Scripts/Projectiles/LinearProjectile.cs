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

    protected override void Start()
    {
        base.Start();

        FaceTarget();
    }

    protected override void Update()
    {
        base.Update();

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

    void FaceTarget()
    {
        transform.right = Target.transform.position - transform.position;
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
