using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointEffector2D), typeof(CircleCollider2D), typeof(Animator))]
public class BlackHole : Attack
{
    [Header("Properties")]
    public float duration;
    public float damageTickTime; // How often to damage enemies within the radius

    [Header("Special")]
    public GameObject explosionEffect;
    public bool isExplosive;
    public bool isShrinking;

    private bool isBeingDestroyed;

    private PointEffector2D pe2d;
    private CircleCollider2D c2d;
    private Animator anim;

    // Always ensures the black hole will slightly pull the target enemy to the left a bit
    // In some cases, the targeted enemy would appear to be standing still when a black hole
    // was summoned over the top of it
    private readonly float xOffset = .5f;

    void Start()
    {
        pe2d = GetComponent<PointEffector2D>();
        c2d = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();

        // Summons the black hole over the target position
        Vector3 summonLocation = new Vector3(Target.transform.position.x + xOffset, Target.transform.position.y, 0);
        transform.position = summonLocation;

        if (!isExplosive)
        {
            StartCoroutine(DamageEnemiesWithin());
        }
    }

    void Update()
    {
        if (isBeingDestroyed)
        {
            return;
        }

        if (duration < 0)
        {
            if (isExplosive)
            {
                StartCoroutine(Explode());
            }
            else
            {
                SelfDestruct();
            }
        }

        duration -= Time.deltaTime;
    }

    void SelfDestruct()
    {
        isBeingDestroyed = true;

        pe2d.enabled = false; // Disables the black hole effect while it is being destroyed

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), c2d.radius);
        foreach (Collider2D nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.isUnderForces = false;
            }
        }

        StartCoroutine(PlayDeathAnimation());
    }

    IEnumerator Explode()
    {
        if (!isBeingDestroyed)
        {
            isBeingDestroyed = true;

            float timeTillExplosion = 1.25f;
            float timeTillDestroyed = 3f;

            pe2d.forceMagnitude *= 2; // Increases the amount of force when exploding

            anim.SetTrigger("Explode");

            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, timeTillDestroyed);

            yield return new WaitForSeconds(timeTillExplosion);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), c2d.radius);
            foreach (Collider2D nearbyObject in colliders)
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                }
            }

            Destroy(gameObject);
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        float timeOfAnimation = 1f;

        anim.SetTrigger("FadeAway");

        yield return new WaitForSeconds(timeOfAnimation);

        Destroy(gameObject);
    }

    IEnumerator DamageEnemiesWithin()
    {
        while (!isBeingDestroyed)
        {
            yield return new WaitForSeconds(damageTickTime);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), c2d.radius);
            foreach (Collider2D nearbyObject in colliders)
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(Damage);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.isUnderForces = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.isUnderForces = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, c2d.radius);
    }
}
