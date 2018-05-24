using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PointEffector2D), typeof(CircleCollider2D), typeof(Animator))]
public class BlackHole : Projectile
{
    public float duration;
    public float damageTickTime; // How often to damage enemies within the radius

    // Always ensures the black hole will slightly pull the target enemy to the left a bit
    // In some cases, the targeted enemy would appear to be standing still when a black hole
    // was summoned over the top of it
    private float xOffset = .5f;

    private bool isBeingDestroyed;

    private PointEffector2D pe2d;
    private CircleCollider2D c2d;
    private Animator anim;

    void Start()
    {
        pe2d = GetComponent<PointEffector2D>();
        c2d = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();

        // Summons the black hole over the target position
        Vector3 summonLocation = new Vector3(Target.transform.position.x + xOffset, Target.transform.position.y, 0);
        transform.position = summonLocation;

        StartCoroutine(DamageEnemiesWithin());
    }

    void Update()
    {   
        if (isBeingDestroyed)
        {
            return;
        }

        if (duration < 0)
        {
            SelfDestruct();
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
