using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : TargetingEntity, IUpgradeable
{
    [Header("Unit Attributes")]
    public Attack attackPrefab;
    public int level = 1;
    public float damage;
    public float attackSpeed;

    [Header("Unit Ability")]
    public bool hasAbility;
    public bool isPassiveCaster;

    [Header("Burst Type")]
    public bool hasBurstAttack;
    public int burstCount;
    public float timeBetweenBursts;
    private float nextAttackTime;

    [Header("Unit Info")]
    public string unitName;
    public int baseCost;
    public Sprite unitSprite;

    [TextArea(5,10)]
    public string description;

    [Header("Upgrade Info")]
    public float multiplier = 1.08f;
    public float upgradeBaseCost;
    internal float upgradeCost;
    public float damageIncrement;

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

    protected virtual void Awake()
    {
        upgradeCost = upgradeBaseCost;      
    }

    protected virtual void Start()
    {
        ResetAttackTime();
    }

    protected virtual void Update()
    {
        if (GameMaster.GameIsOver)
        {
            return;
        }

        if (Spawner.EnemiesAlive <= 0)
        {
            ResetAttackTime();
            return;
        }

        if (nextAttackTime > attackSpeed)
        {
            if (hasAbility && !isPassiveCaster)
            {
                return;
            }

            if (hasBurstAttack)
            {
                StartCoroutine(BurstAttack());
            }
            else
            {
                Attack();
            }

            ResetAttackTime();
        }

        nextAttackTime += Time.deltaTime;
    }

    public void ResetAttackTime()
    {
        nextAttackTime = 0;
    }

    bool IsTargetAvailable()
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

                        if (target != null)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public GameObject ObtainTarget()
    {
        switch (unitAI)
        {
            case AIType.Nearest:
                if (TargetNearestEnemy() != null) return TargetNearestEnemy();
                break;
            case AIType.Random:
                if (TargetRandomEnemy() != null) return TargetRandomEnemy();
                break;
            case AIType.Dot:
                if (TargetEnemyForDoT() != null) return TargetEnemyForDoT();
                break;
            default:
                Debug.Log("No target available for " + unitName + "!");
                break;
        }

        return null;
    }

    public void Attack()
    {
        if (IsTargetAvailable())
        {
            Attack unitAttack = Instantiate(attackPrefab, transform.position, Quaternion.identity);

            if (unitAttack != null)
            {
                unitAttack.originEntity = gameObject; // Indicates the exact unit the attack came from
                unitAttack.Damage = damage; // Sets the damage of the attack
                unitAttack.Target = target; // Sets the target for the attack
                unitAttack.CanAttackFlying = canAttackFlying;
            }
        }
    }

    public IEnumerator BurstAttack()
    {
        for(int i = 0; i < burstCount; i++)
        {        
            Attack();
            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    public void Upgrade()
    {
        level++;
        damage += damageIncrement;

        float upgradedCost = upgradeBaseCost * Mathf.Pow(multiplier, level);

        upgradeCost = (int)upgradedCost;
    }

    public void SetLevel(int levelToSetTo)
    {
        level = levelToSetTo;
        damage += damageIncrement * level;

        if (level > 1)
        {
            float upgradedCost = upgradeBaseCost * Mathf.Pow(multiplier, level);
            upgradeCost = (int)upgradedCost;
        }
    }

    public void Toggle()
    {
        // Disables or enables unit based on current state
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public float GetNextAttackTime()
    {
        return nextAttackTime;
    }
}
