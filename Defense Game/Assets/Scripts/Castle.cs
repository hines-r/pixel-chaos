using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public void TakeDamage(float damage)
    {
        PlayerStats.Health -= damage;

        if (PlayerStats.Health <= 0)
        {
            PlayerStats.Health = 0;
        }
    }
}
