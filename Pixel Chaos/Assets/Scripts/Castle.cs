using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage)
    {
        Player.instance.health -= damage;

        if (Player.instance.health <= 0)
        {
            Player.instance.health = 0;
        }
    }
}
