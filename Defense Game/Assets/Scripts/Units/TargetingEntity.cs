using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingEntity : MonoBehaviour
{
    protected float maxAttackRange = 8f; // Can attack enemies when their x is less than this many world units
    protected float maxYAttackRanage = 0.5f; // Can only attack enemies when they're below this y point

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
            if (dSqrtToTarget < closestDistanceSqr && enemy.transform.position.y <= maxYAttackRanage)
            {
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
            LivingEntity livingEnemy = enemy.GetComponent<Enemy>();

            if (livingEnemy != null)
            {
                if (!livingEnemy.isDamagedOverTime)
                {
                    enemiesWithDoT.Add(livingEnemy.gameObject);
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
