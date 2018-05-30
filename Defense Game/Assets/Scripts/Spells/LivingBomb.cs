using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingBomb : Attack
{
    [Header("Effects")]
    public GameObject explosionEffect;
    public GameObject explosionAfterSpread;

    [Header("Bomb Properties")]
    public float splashRadius;
    public float burnMultiplier = 5f; // Burn damage multiplier
    public float initialDetonationTime = 3f;

    public int timesToSpread = 4; // Times the bomb can spread the explosion
    public int spreadNumber = 3; // Number of enemies to spread the bomb to

    [Header("Secondary Bomb Random Time")]
    public float detonationTimeMin = 1f; // The time it takes the bomb to detonate after it has spread
    public float detonationTimeMax = 3f;

    private Enemy enemy;

    void Start()
    {
        if (Target != null)
        {
            enemy = Target.GetComponent<Enemy>();

            if (enemy != null && !enemy.isLivingBomb)
            {
                transform.position = enemy.transform.position;
                StartBomb(initialDetonationTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void LateUpdate()
    {
        if (enemy != null)
        {
            transform.position = enemy.transform.position; // Follows the enemy until detonation
            enemy.TakeDamage(Damage * burnMultiplier * Time.deltaTime); // Burns the enemy while active

            if (!enemy.isLivingBomb)
            {
                Explode(); // Explodes once the enemy debuff fades
            }
        }
        else
        {
            Explode(); // Explodes if the enemy dies
        }
    }

    void StartBomb(float _bombDetonationTime)
    {
        if (enemy != null && !enemy.isLivingBomb)
        {
            enemy.ApplyBomb(_bombDetonationTime);
        }
    }

    void Explode()
    {
        float timeTillDestroyed = 1.5f;

        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, timeTillDestroyed);

        timesToSpread--;

        int count = 0;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), splashRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemyInSplashRadius = nearbyObject.GetComponent<Enemy>();

            if (enemyInSplashRadius != null && !enemyInSplashRadius.isLivingBomb)
            {
                if (timesToSpread > 0)
                {
                    if (count < spreadNumber)
                    {
                        if (enemy != null)
                        {
                            if (enemy.GetInstanceID() != enemyInSplashRadius.GetInstanceID())
                            {
                                SpreadBomb(enemyInSplashRadius); // Spreads the bomb to other enemies other than the original bomb
                            }
                        }
                        else
                        {
                            SpreadBomb(enemyInSplashRadius); // The original bomb carrier has died and the bomb can spread to others
                        }

                        count++; // The amount of enemies the bomb has spread to in one explosion
                    }
                }

                enemyInSplashRadius.TakeDamage(Damage);
            }
        }

        Destroy(gameObject);
    }

    void SpreadBomb(Enemy enemyToSpreadTo)
    {
        LivingBomb newBomb = Instantiate(this, enemyToSpreadTo.transform.position, Quaternion.identity);
        newBomb.Damage = Damage;
        newBomb.Target = enemyToSpreadTo.gameObject;
        newBomb.timesToSpread = timesToSpread;
        newBomb.explosionEffect = explosionAfterSpread;
        newBomb.initialDetonationTime = Random.Range(detonationTimeMin, detonationTimeMax);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
