using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    [Header("Enemy Attributes")]
    public float startSpeed = 1;
    private float speed;

    public float damage = 5f;
    public float attackSpeed = 1f;
    public float timeBetweenAttacks = 1f;
    private float nextAttackTime;
    public float lungeDistance = 3f;

    [Header("Enemy Misc.")]
    public float stoppingPoint = -1;
    public int value = 10;
    public int expAmount = 5;

    [Header("Death Effect (Optional)")]
    public GameObject deathEffect;

    private float velocity;
    private Vector3 previousPosition;

    private Transform target;

    private SpriteRenderer spriteRenderer;

    public enum State { Moving, Attacking }
    private State currentState;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = startSpeed;
        currentState = State.Moving;
    }

    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            if (currentState == State.Attacking)
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

        if (transform.position.x >= stoppingPoint && currentState == State.Moving)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        else
        {
            currentState = State.Attacking;
        }

        speed = startSpeed; // Used to revert back to normal movement speed when a slow ends
    }

    IEnumerator Attack()
    {
        lungeDistance = Random.Range(.5f, 1.25f);

        Vector3 originalPos = transform.position;
        Vector3 attackPos = new Vector3(stoppingPoint - lungeDistance, transform.position.y, 0);

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

    public void Slow(float percentage)
    {
        speed = startSpeed * (1f - percentage);
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
            float enemyHeight = spriteRenderer.bounds.size.y;
            Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y + enemyHeight / 2);

            GameObject income = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Text incomeText = income.GetComponentInChildren<Text>();
            incomeText.text = "+" + value + "g";

            Destroy(income, 2f);
        }

        PlayerStats.Gold += value;
        PlayerStats.Experience += expAmount;
    }
}
