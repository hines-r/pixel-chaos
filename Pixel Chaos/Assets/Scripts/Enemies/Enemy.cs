﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Enemy : LivingEntity
{
    [Header("Enemy Attributes")]
    public Type enemyType = Type.Ground;
    private readonly float altitude = 3.5f; // Simulated distance above the ground (how high the flyer actually is)
    public float startSpeed = 1;
    private float speed;
    public float damage = 5f;
    public float attackSpeed = 1f;
    public float timeBetweenAttacks = 1f;
    private float nextAttackTime;

    // Debuffs
    private float slowDuration;

    private bool isStunned;
    private float stunDuration;

    internal bool isLivingBomb;
    private float bombDetonationTime;

    private bool isAirborne;
    private Vector3 airbornePosition;
    private readonly float airborneYThreshold = 0.25f; // Maximum y position until considered airborne
    private readonly float minYPosition = -5f; // Minimum y position enemy can fall to

    internal bool isTrapped;

    internal bool isUnderForces;

    // Attack animation
    private readonly float minLungeDistance = 0.5f;
    private readonly float maxLungeDistance = 1.5f;

    [Header("Enemy Misc.")]
    public float stoppingPoint = -1;
    public int goldValue = 10;
    public int expAmount = 5;

    [Header("Death Effect (Optional)")]
    public GameObject deathEffect;

    private Vector3 velocity;
    private Vector3 previousPosition;

    [Header("Debuffs")]
    public GameObject poisonDebuff;
    public GameObject slowDebuff;
    public GameObject stunnedDebuff;
    public GameObject bombDebuff;
    public GameObject airborneDebuff;
    public GameObject trappedDebuff;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public enum State { Moving, Attacking, UnderForces }
    public enum Type { Ground, Flying }

    internal State currentState;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        speed = startSpeed;
        currentState = State.Moving;

        goldValue = EnemyScaler.ScaleGold(goldValue, Spawner.WaveIndex - 1);
        expAmount = EnemyScaler.ScaleExpValue(expAmount, Spawner.WaveIndex - 1);
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

        if (bombDetonationTime > 0)
        {
            bombDebuff.SetActive(true);
            bombDetonationTime -= Time.deltaTime;
        }
        else
        {
            isLivingBomb = false;
            bombDebuff.SetActive(false);
        }

        if (isTrapped)
        {
            trappedDebuff.SetActive(true);
        }
        else
        {
            trappedDebuff.SetActive(false);
        }

        if (transform.position.y > airborneYThreshold && !isAirborne)
        {
            if (enemyType != Type.Flying)
            {
                isAirborne = true;
            }
        }

        if (isAirborne)
        {
            airborneDebuff.SetActive(true);

            // The enemy has landed at its initial y position before becoming airborne
            if (transform.position.y < airbornePosition.y)
            {
                isAirborne = false;
                isUnderForces = false;
                airborneDebuff.SetActive(false);
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
        }

        if (!isUnderForces && isAirborne)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y); // If an enemy ends up above the airborne threshold, allows them to fall back down
        }
        else if (!isUnderForces)
        {
            rb.velocity = Vector2.zero;  // Sets velocity to 0 whenever a force has ended
        }

        if (Time.time > nextAttackTime && currentState == State.Attacking)
        {
            // Can't attack if under the effects of a black hole or other force
            if (!isUnderForces && !isStunned && !isAirborne && !isTrapped)
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    void FixedUpdate()
    {
        velocity = (previousPosition - transform.position) / Time.deltaTime;
        previousPosition = transform.position;

        if (enemyType == Type.Flying && !isAirborne)
        {
            airbornePosition = new Vector3(transform.position.x, transform.position.y - altitude);
        }

        if (transform.position.x >= stoppingPoint)
        {
            currentState = State.Moving;

            // Can only move is not stunned or airborne
            if (!isStunned && !isAirborne && !isTrapped)
            {
                transform.position -= transform.right * speed * Time.deltaTime;
            }
        }
        else
        {
            if (!isUnderForces && !isStunned && !isAirborne)
            {
                currentState = State.Attacking;
            }
        }

        // Checks if the enemy has decended below the visible screen
        // If so, sets the y position at the very bottom adjusted by the sprites height
        if (transform.position.y < minYPosition + sr.bounds.size.y / 2)
        {
            transform.position = new Vector3(transform.position.x, minYPosition + sr.bounds.size.y / 2, transform.position.z);
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

        Player.instance.health -= damage;

        if (Player.instance.health <= 0)
        {
            Player.instance.health = 0;
        }
    }

    public void Slow(float percentage, float _slowDuration)
    {
        speed = startSpeed * (1f - percentage);
        slowDuration = _slowDuration;
    }

    public void Stun(float _stunDuration)
    {
        isStunned = true;
        stunDuration = _stunDuration;
    }

    public void ApplyBomb(float _bombDetonationTime)
    {
        if (!isLivingBomb)
        {
            isLivingBomb = true;
            bombDetonationTime = _bombDetonationTime;
        }
    }

    public void MakeAirborne(float xForce, float yForce)
    {
        if (!isAirborne)
        {
            isAirborne = true;
            airbornePosition = transform.position; // The position before being airborne
        }

        rb.gravityScale = 1;
        rb.AddForce(new Vector2(xForce, yForce));
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

        Player.instance.gold += goldValue;
        Player.instance.experience += expAmount;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rb;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return sr;
    }

    public bool IsAirborne()
    {
        return isAirborne;
    }

    public float GetAirborneYThreshold()
    {
        return airborneYThreshold;
    }

    public Vector3 GetAirbornePosition()
    {
        return airbornePosition;
    }
}
