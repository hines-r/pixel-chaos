using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Attack
{
    private readonly float yOffset = 1f;

    void Start()
    {
        Vector2 screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        transform.position = new Vector3(Target.transform.position.x, screenHalfSizeWorldUnits.y + yOffset);

        Destroy(gameObject, 5f);
    }

    void OnParticleCollision(GameObject collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(Damage);
        }
    }

}
