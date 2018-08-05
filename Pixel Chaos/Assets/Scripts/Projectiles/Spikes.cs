using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : ParabolicProjectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (isOnGround)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null && !enemy.IsAirborne() || enemy.enemyType != Enemy.Type.Flying)
            {
                enemy.GetRigidbody2D().sleepMode = RigidbodySleepMode2D.NeverSleep;

                if (enemy.enemyType != Enemy.Type.Flying || !enemy.IsAirborne())
                {
                    if (slowAmount > 0f)
                    {
                        enemy.Slow(slowAmount, slowDuration);
                    }

                    enemy.TakeDamage(Damage * Time.deltaTime);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isOnGround)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.GetRigidbody2D().sleepMode = RigidbodySleepMode2D.StartAwake;
            }
        }

    }
}
