using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miasma : Attack
{
    public float radius;
    public float duration;

    private readonly float timeBetweenApplication = .5f; // How often to apply the poison

    void Start()
    {
        transform.position = Target.transform.position;

        StartCoroutine(SpawnMist());

        Destroy(gameObject, duration);
    }

    IEnumerator SpawnMist()
    {
        while (true)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius);

            foreach (Collider2D nearbyObject in colliders)
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.ApplyDoT(Damage, duration, false);
                }
            }

            yield return new WaitForSeconds(timeBetweenApplication);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
