using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Cage : Attack
{
    internal float fallSpeed;
    internal float duration;
    internal Vector3 impactLocation;

    private bool isLanded;

    private Collider2D c2d;

    private HashSet<Enemy> enemiesTrapped;

    void Start()
    {
        c2d = GetComponent<Collider2D>();
        enemiesTrapped = new HashSet<Enemy>();

        Enemy enemyTarget = Target.GetComponent<Enemy>();

        if (enemyTarget != null)
        {
            transform.localScale = enemyTarget.gameObject.transform.localScale * 1.5f;
        }

        StartCoroutine(StartDestroy());
    }

    void Update()
    {
        if (isLanded)
        {
            return;
        }

        if (transform.position.y < impactLocation.y)
        {
            HitGround();
        }

        if (!isLanded)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        }
    }

    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(duration);

        foreach (Enemy enemy in enemiesTrapped)
        {
            enemy.isTrapped = false;
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLanded)
        {
            Enemy enemyWithin = collision.GetComponent<Enemy>();

            if (enemyWithin != null)
            {
                enemiesTrapped.Add(enemyWithin);
            }
        }
    }

    void HitGround()
    {
        isLanded = true;

        foreach (Enemy enemy in enemiesTrapped)
        {
            SpriteRenderer enemyBounds = enemy.GetSpriteRenderer();

            if (enemyBounds != null)
            {               
                if (c2d.bounds.Contains(enemyBounds.bounds))
                {
                    enemy.TakeDamage(Damage);
                    enemy.isTrapped = true;
                }
            }
        }
    }
}
