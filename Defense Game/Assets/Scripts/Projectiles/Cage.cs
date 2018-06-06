using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : Attack
{
    public Collider2D[] bars;

    internal float fallSpeed;
    internal float duration;

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
            transform.localScale = enemyTarget.gameObject.transform.localScale * 2f;
        }

        StartCoroutine(StartDestroy());
    }

    void Update()
    {
        if (isLanded)
        {
            return;
        }

        if (transform.position.y < Target.transform.position.y)
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

        List<Enemy> enemiesToFree = new List<Enemy>(enemiesTrapped);

        foreach (Enemy enemy in enemiesToFree)
        {
            enemy.isTrapped = false;
        }

        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemyWithin = collision.GetComponent<Enemy>();

        if (enemyWithin != null)
        {
            enemyWithin.isTrapped = true;
            enemiesTrapped.Add(enemyWithin);
        }
    }

    void HitGround()
    {
        foreach (Collider2D collider in bars)
        {
            collider.enabled = false;
        }

        c2d.enabled = false;
        isLanded = true;
    }
}
