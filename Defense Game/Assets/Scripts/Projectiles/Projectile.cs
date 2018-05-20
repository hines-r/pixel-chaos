using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal GameObject originEntity;

    public float Damage { get; set; }

    public GameObject Target { get; set; }
}
