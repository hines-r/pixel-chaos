using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Projectile
{
    private Transform target;

    private float yOffset = 1f;

    void Start()
    {
        target = NearestTarget().transform;
        Vector2 screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        transform.position = new Vector3(target.position.x, screenHalfSizeWorldUnits.y + yOffset);

        Destroy(gameObject, 5f);
    }

}
