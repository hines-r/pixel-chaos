using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class TidalWave : Attack
{
    [Header("Properties")]
    public float speed = 5f;
    public float distanceXTillFade = 0f;

    [Space]
    public bool hasKnockup;
    public float xForce = 0f;
    public float yForce = 0f;

    private readonly float startPositionX = -4f;
    private bool isBeingDestroyed;

    private Animator anim;
    private Collider2D c2d;

    void Start()
    {
        anim = GetComponent<Animator>();
        c2d = GetComponent<Collider2D>();

        if (Target != null)
        {
            transform.position = new Vector3(startPositionX, Target.transform.position.y, Target.transform.position.z);
        }
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        if (!isBeingDestroyed && transform.position.x >= distanceXTillFade)
        {
            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        isBeingDestroyed = true;

        float timeTillDisable = 2f;
        float timeTillDestroy = 3.5f;

        anim.SetTrigger("Destroy");

        yield return new WaitForSeconds(timeTillDisable);

        c2d.enabled = false;

        yield return new WaitForSeconds(timeTillDestroy - timeTillDisable);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (hasKnockup)
            {
                enemy.isUnderForces = true;
                enemy.KnockUp(xForce, yForce);
            }

            enemy.TakeDamage(Damage);
        }
    }
}
