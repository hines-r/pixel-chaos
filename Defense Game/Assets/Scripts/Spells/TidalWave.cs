using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TidalWave : Attack
{
    [Header("Properties")]
    public float speed = 5f;

    private readonly float startPositionX = -4f;
    private readonly float distanceXTillFade = 0f;

    private Animator anim;
    private BoxCollider2D bc2d;

    void Start()
    {
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();

        transform.position = new Vector3(startPositionX, Target.transform.position.y, Target.transform.position.z);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        if (transform.position.x >= distanceXTillFade)
        {
            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        float timeTillDisable = 2f;
        float timeTillDestroy = 3.5f;

        anim.SetTrigger("Destroy");

        yield return new WaitForSeconds(timeTillDisable);

        bc2d.enabled = false;

        yield return new WaitForSeconds(timeTillDestroy - timeTillDisable);

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        enemy.TakeDamage(Damage);
    }
}
