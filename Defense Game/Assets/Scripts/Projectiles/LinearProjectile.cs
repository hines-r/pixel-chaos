using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : Projectile
{
    [Header("Properties")]
    public float speed = 25f;

    protected override void Start()
    {
        base.Start();

        transform.right = Target.transform.position - transform.position;
    }

    protected override void Update()
    {
        base.Update();

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
