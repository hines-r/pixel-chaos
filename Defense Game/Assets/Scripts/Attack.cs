using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    internal GameObject originEntity;

    [Header("Friendly or Hostile Projectile")]
    public bool isHostile; // Determines if the attack is coming from a friendly unit or an enemy

    public float Damage { get; set; }

    public GameObject Target { get; set; }
}
