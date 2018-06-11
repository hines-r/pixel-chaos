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
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.GetRigidbody2D().sleepMode = RigidbodySleepMode2D.NeverSleep;

            if (slowAmount > 0f)
            {
                enemy.Slow(slowAmount, slowDuration);
            }

            enemy.TakeDamage(Damage * Time.deltaTime);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.GetRigidbody2D().sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }
}
