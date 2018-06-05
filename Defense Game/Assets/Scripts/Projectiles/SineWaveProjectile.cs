using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveProjectile : Projectile
{
    [Header("Properties")]
    public float speed = 10f;
    public float frequency = 2f;
    public float magnitude = 0.5f;

    private Vector3 axis;
    private Vector3 pos;

    protected override void Start()
    {
        base.Start();

        FaceTarget();

        pos = transform.position;
        axis = transform.up;
    }

    protected override void Update()
    {
        base.Update();

        pos += transform.right * Time.deltaTime * speed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    void FaceTarget()
    {
        if (Target != null)
        {
            transform.right = Target.transform.position - transform.position;
        }
    }
}
