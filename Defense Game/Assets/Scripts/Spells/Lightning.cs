using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Attack
{
    public ParticleSystem particles;

    [Header("Properties")]
    public bool hasStun;
    public float stunDuration;

    
    private float particleTime = 5f;

    void Start()
    {
        Vector2 screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);

        float yOffset = 1f;
        float xOffset = particles.shape.radius / 3;

        print(xOffset);
        transform.position = new Vector3(Target.transform.position.x + xOffset, screenHalfSizeWorldUnits.y + yOffset);

        Destroy(gameObject, particleTime);
    }

    void OnParticleCollision(GameObject collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            if (hasStun)
            {
                enemy.Stun(stunDuration);
            }

            enemy.TakeDamage(Damage);
        }
    }
}
