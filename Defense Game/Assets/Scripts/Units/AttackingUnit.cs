using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingUnit : MonoBehaviour
{
    [Header("Unit Attributes")]
    public GameObject projectile;
    public float damage;
    public float attackSpeed;

    [Header("Unit Info")]
    public string unitName;
    public int baseCost;
    public string description;
    public Sprite unitSprite;

    [Header("Upgrade Info")]
    public float multiplier = 1.08f;
    public float upgradeBaseCost;
    private float upgradeCost;
    public float damageIncrement;

    private float nextAttackTime;
    private float maxAttackRange = 8f; // Can attack enemies when their x is less than this many world units

    internal int level = 1;

    void Start()
    {
        upgradeCost = upgradeBaseCost;
        nextAttackTime = attackSpeed;
    }

    void Update()
    {
        if (GameMaster.GameIsOver)
        {
            return;
        }

        if (EnemySpawner.EnemiesAlive <= 0)
        {
            return;
        }

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackSpeed;
            Attack();
        }
    }

    bool CheckForTargets()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");

        if (possibleTargets.Length > 0)
        {
            foreach (GameObject obj in possibleTargets)
            {
                Enemy enemy = obj.GetComponent<Enemy>();

                if (enemy != null)
                {
                    // If an enemy is found and it is within the attack range, there is a valid target
                    if (enemy.transform.position.x < maxAttackRange)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void Attack()
    {
        if (CheckForTargets())
        {
            GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity);
            Projectile p = obj.GetComponent<Projectile>();

            if (p != null)
            {
                p.Damage = damage; // Sets the damage of the projectile being fired
            }
        }
    }

    public void Upgrade()
    {
        level++;
        damage += damageIncrement;

        float upgradedCost = upgradeBaseCost * Mathf.Pow(multiplier, level);

        upgradeCost = (int)upgradedCost;
    }

}
