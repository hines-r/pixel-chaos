using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingProjectile : Projectile
{
    [Header("Properties")]
    public float speed;

    [Header("Simulated Impact Location")]
    public float yMinImpact = 0;
    public float yMaxImpact = 5;

    private Vector3 impactLocation;

    protected override void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, -90); // Faces the projectile straight down

        float randomImpactY = Random.Range(yMinImpact, yMaxImpact);
        impactLocation = new Vector3(transform.position.x, randomImpactY, transform.position.z);
    }

    protected override void Update()
    {
        if (transform.position.y < impactLocation.y)
        {
            HitGround();
        }

        transform.position += transform.right * speed * Time.deltaTime;
    }

    void HitGround()
    {
        if (impactEffect != null)
        {
            Impact();
        }

        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawLine(new Vector3(transform.position.x, yMinImpact), new Vector3(transform.position.x, yMaxImpact));
    }
}
