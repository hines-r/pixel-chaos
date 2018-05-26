using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : LivingEntity
{
    [Header("Enemy Attributes")]
    public float startSpeed = 1;
    private float speed;

    public float damage = 5f;
    public float attackSpeed = 1f;
    public float timeBetweenAttacks = 1f;
    private float nextAttackTime;

    // Debuffs
    private bool isSlowed;
    private float slowDuration;

    private bool isStunned;
    private float stunDuration;

    // Attack animation
    private readonly float minLungeDistance = 0.5f;
    private readonly float maxLungeDistance = 1.5f;

    [Header("Enemy Misc.")]
    public float stoppingPoint = -1;
    public int goldValue = 10;
    public int expAmount = 5;

    [Header("Death Effect (Optional)")]
    public GameObject deathEffect;

    private float velocity;
    private Vector3 previousPosition;

    [Header("Debuffs")]
    public GameObject poisonDebuff;
    public GameObject slowDebuff;
    public GameObject stunnedDebuff;

    internal bool isUnderForces;

    private Rigidbody2D rb;

    public enum State { Moving, Attacking, UnderForces }
    internal State currentState;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        speed = startSpeed;
        currentState = State.Moving;

        goldValue = EnemyScaler.ScaleGold(goldValue, ProceduralSpawner.WaveIndex - 1);
        expAmount = EnemyScaler.ScaleExpValue(expAmount, ProceduralSpawner.WaveIndex - 1);
    }

    void Update()
    {
        if (isDamagedOverTime)
        {
            poisonDebuff.SetActive(true);
            dotDuration -= Time.deltaTime;

            TakeDamageOverTime();

            if (dotDuration <= 0)
            {
                poisonDebuff.SetActive(false);
                isDamagedOverTime = false;
            }
        }

        if (slowDuration > 0)
        {
            slowDebuff.SetActive(true);
            slowDuration -= Time.deltaTime;
        }
        else
        {
            isSlowed = false;
            slowDebuff.SetActive(false);
            speed = startSpeed;
        }

        if (stunDuration > 0)
        {
            stunnedDebuff.SetActive(true);
            stunDuration -= Time.deltaTime;
        }
        else
        {
            isStunned = false;
            stunnedDebuff.SetActive(false);
        }

        if (!isUnderForces)
        {
            rb.velocity = Vector2.zero; // Sets velocity to 0 after exiting a black hole
        }

        if (Time.time > nextAttackTime && currentState == State.Attacking)
        {
            // Can't attack if under the effects of a black hole or other force
            if (!isUnderForces)
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    void FixedUpdate()
    {
        velocity = ((transform.position - previousPosition).magnitude) / Time.deltaTime;
        previousPosition = transform.position;

        if (transform.position.x >= stoppingPoint)
        {
            currentState = State.Moving;

            // Can only move is not stunned
            if (!isStunned)
            {
                transform.position -= transform.right * speed * Time.deltaTime;
            }
        }
        else
        {
            if (!isUnderForces && !isStunned)
            {
                currentState = State.Attacking;
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        float lunge = Random.Range(minLungeDistance, maxLungeDistance);

        Vector3 originalPos = transform.position;
        Vector3 attackPos = new Vector3(stoppingPoint - lunge, transform.position.y, 0);

        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPos, attackPos, interpolation);

            yield return null;
        }

        PlayerStats.Health -= damage;

        if (PlayerStats.Health <= 0)
        {
            PlayerStats.Health = 0;
        }
    }

    public void Slow(float percentage, float _slowDuration)
    {
        isSlowed = true;
        speed = startSpeed * (1f - percentage);
        slowDuration = _slowDuration;
    }

    public void Stun(float _stunDuration)
    {
        isStunned = true;
        stunDuration = _stunDuration;
    }

    public float GetVelocity()
    {
        return velocity;
    }

    protected override void Die()
    {
        base.Die();

        if (deathEffect != null)
        {
            GameObject income = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Text incomeText = income.GetComponentInChildren<Text>();
            incomeText.text = "+" + goldValue + "g";

            float effectTime = 1f;

            Destroy(income, effectTime);
        }

        PlayerStats.Gold += goldValue;
        PlayerStats.Experience += expAmount;
    }
}
