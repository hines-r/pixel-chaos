using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingProjectile : Projectile
{
    [Header("Properties")]
    public float speed = 5f;
    public float rotationSpeed = 200f;

    private Rigidbody2D rb;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Target != null)
        {
            // Rotates the projectile to face the target upon instantiation
            Vector3 diff = Target.transform.position - transform.position;
            diff.Normalize();

            float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90f);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Target == null)
        {
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        AttackingUnit unit = originEntity.GetComponent<AttackingUnit>();

        if (unit != null)
        {
            Target = unit.ObtainTarget();
        }
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector2 direction = (Vector2)Target.transform.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotationSpeed;
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.velocity = transform.up * speed; // Keeps traveling in the foward direction until new target is aquired
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
