using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        PlayerStats.instance.health -= damage;

        if (PlayerStats.instance.health <= 0)
        {
            PlayerStats.instance.health = 0;
        }
    }
}
