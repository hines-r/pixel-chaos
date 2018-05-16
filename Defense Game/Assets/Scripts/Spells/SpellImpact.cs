using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellImpact : MonoBehaviour
{
    public Projectile spell;

    void OnParticleCollision(GameObject collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(spell.Damage);
        }
    }
}
