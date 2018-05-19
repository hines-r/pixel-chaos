﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Projectile
{
    private float yOffset = 1f;

    void Start()
    {
        Vector2 screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        transform.position = new Vector3(Target.position.x, screenHalfSizeWorldUnits.y + yOffset);

        Destroy(gameObject, 5f);
    }

}
