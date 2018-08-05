using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionMagic : Attack
{
    [Header("Properties")]
    public float splashRadius = 5f;
    public bool isRandomized;
    public float randomOffset;

    private float timeAlive = 4f;

    void Start()
    {
        if (isRandomized)
        {
            Vector3 randomizedPosition = new Vector3(Target.transform.position.x + Random.Range(-randomOffset, randomOffset),
                Target.transform.position.y + Random.Range(-randomOffset, randomOffset), Target.transform.position.z);

            transform.position = randomizedPosition;

            timeAlive /= 3;
        }
        else
        {
            transform.position = Target.transform.position;
        }

        Explode();
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), splashRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(Damage);
            }
        }

        Destroy(gameObject, timeAlive);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
