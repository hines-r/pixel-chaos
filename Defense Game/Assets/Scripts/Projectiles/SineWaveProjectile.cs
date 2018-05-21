using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveProjectile : Projectile
{
    public float speed = 10f;
    public float frequency = 2f;
    public float magnitude = 0.5f;

    private Vector3 axis;
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
        axis = transform.up;
    }

    void Update()
    {
        pos += transform.right * Time.deltaTime * speed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
