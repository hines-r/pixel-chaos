using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingUnit : TargetingEntity
{
    [Header("Unit Attributes")]
    public GameObject projectile;
    public int level = 1;
    public float damage;
    public float attackSpeed;
    protected float nextAttackTime;

    [Header("Unit Info")]
    public string unitName;
    public int baseCost;
    public string description;
    public Sprite unitSprite;

    [Header("Upgrade Info")]
    public float multiplier = 1.08f;
    public float upgradeBaseCost;
    internal float upgradeCost;
    public float damageIncrement;

    internal float maxAttackRange = 8f; // Can attack enemies when their x is less than this many world units

    public enum AIType
    {
        Nearest,
        Random,
        Dot // Used to target nearest enemies without a DoT (damage over time) effect
    }

    [Header("Artificial Intelligence")]
    public AIType unitAI;

    internal Node currentNode; // The node the unit is currently placed on
    private GameObject target;

    protected virtual void Start()
    {
        upgradeCost = upgradeBaseCost;
        nextAttackTime = 0;
    }

    protected virtual void Update()
    {
        if (GameMaster.GameIsOver)
        {
            return;
        }

        if (ProceduralSpawner.EnemiesAlive <= 0)
        {
            return;
        }

        if (nextAttackTime > attackSpeed)
        {
            nextAttackTime = 0;
            Attack();
        }

        nextAttackTime += Time.deltaTime;
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
                        target = ObtainTarget();
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public GameObject ObtainTarget()
    {
        if (unitAI == AIType.Nearest)
        {
            if (TargetNearestEnemy() != null)
            {
                return TargetNearestEnemy();
            }
        }
        else if (unitAI == AIType.Random)
        {
            if (TargetRandomEnemy(this) != null)
            {
                return TargetRandomEnemy(this);
            }
        }
        else if (unitAI == AIType.Dot)
        {
            if (TargetEnemyForDoT() != null)
            {
                return TargetEnemyForDoT();
            }
        }
        else
        {
            Debug.Log(this + ": Unit needs to specify targeting AI!");
        }

        return null;
    }

    void Attack()
    {
        if (CheckForTargets())
        {
            GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity);
            Projectile projectileToFire = obj.GetComponent<Projectile>();

            if (projectileToFire != null)
            {
                projectileToFire.originEntity = gameObject;
                projectileToFire.Damage = damage; // Sets the damage of the projectile being fired
                projectileToFire.Target = target; // Sets the target for the projectile
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
