using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth;
    public Image healthBar;
    protected float health;
    protected bool isDead;

    internal bool isDamagedOverTime;
    private float dotDamage;
    protected float dotDuration;

    protected virtual void Start()
    {
        startingHealth = EnemyScaler.ScaleHealth(startingHealth, ProceduralSpawner.WaveIndex - 1);
        health = startingHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.fillAmount = health / startingHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamageOverTime()
    {
        health -= dotDamage * Time.deltaTime;

        healthBar.fillAmount = health / startingHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void ApplyDoT(float damage, float duration, bool isStacking)
    {
        isDamagedOverTime = true;
        dotDuration = duration;

        if (isStacking)
        {
            dotDamage += damage;
        }
        else
        {
            dotDamage = damage;
        }
    }

    protected virtual void Die()
    {
        isDead = true;

        Destroy(gameObject);
        ProceduralSpawner.EnemiesAlive--;
    }

}
