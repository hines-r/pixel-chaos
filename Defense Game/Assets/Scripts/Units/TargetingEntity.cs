using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingEntity : MonoBehaviour
{
    public bool canAttackFlying;
    protected float maxAttackRange = 8f; // Can attack enemies when their x is less than this many world units

    protected GameObject TargetNearestEnemy()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> listOfPossibleTargets = new List<GameObject>(possibleTargets);

        return FindNearestTargetInList(listOfPossibleTargets);
    }

    GameObject FindNearestTargetInList(List<GameObject> listToSearch)
    {
        GameObject nearestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in listToSearch)
        {
            Vector3 directionToTarget = enemy.transform.position - currentPosition;
            float dSqrtToTarget = directionToTarget.sqrMagnitude;

            if (dSqrtToTarget < closestDistanceSqr)
            {
                Enemy e = enemy.GetComponent<Enemy>();

                if (!canAttackFlying && (e.IsAirborne() || e.enemyType == Enemy.Type.Flying))
                {
                    continue;
                }

                closestDistanceSqr = dSqrtToTarget;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    protected GameObject TargetRandomEnemy()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesInRange = new List<GameObject>();

        foreach (GameObject enemy in possibleTargets)
        {
            // Random target must be within attack range of the unit
            if (enemy.transform.position.x <= maxAttackRange)
            {
                Enemy e = enemy.GetComponent<Enemy>();

                if (!canAttackFlying && (e.IsAirborne() || e.enemyType == Enemy.Type.Flying))
                {
                    continue;
                }

                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count > 0)
        {
            GameObject randomTarget = enemiesInRange[Random.Range(0, enemiesInRange.Count - 1)];
            return randomTarget;
        }

        return null;
    }

    protected GameObject TargetEnemyForDoT()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> enemiesWithDoT = new List<GameObject>();

        foreach (GameObject enemy in possibleTargets)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            if (e != null)
            {
                if (!e.isDamagedOverTime)
                {
                    if (!canAttackFlying && (e.IsAirborne() || e.enemyType == Enemy.Type.Flying))
                    {
                        continue;
                    }

                    enemiesWithDoT.Add(e.gameObject);
                }
            }
        }

        // Attempts to find the nearest target without a DoT
        if (enemiesWithDoT.Count > 0)
        {
            return FindNearestTargetInList(enemiesWithDoT);
        }

        // Defaults to nearest target without searching for dot if none are found
        return TargetNearestEnemy();
    }
}
