using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage { get; set; }

    protected GameObject NearestTarget()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in possibleTargets)
        {
            Vector3 directionToTarget = enemy.transform.position - currentPosition;
            float dSqrtToTarget = directionToTarget.sqrMagnitude;
            if (dSqrtToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrtToTarget;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            //nearestEnemy.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        return nearestEnemy;
    }

    protected GameObject RandomTarget()
    {
        GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag("Enemy");

        if (possibleTargets.Length > 0)
        {
            GameObject randomTarget = possibleTargets[Random.Range(0, possibleTargets.Length - 1)];
            return randomTarget;
        }

        return null;
    }

    protected GameObject NoDotTarget()
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
            return FindNearestInList(enemiesWithDoT);
        }

        // Defaults to nearest target without searching for dot if none are found
        return NearestTarget();
    }

    GameObject FindNearestInList(List<GameObject> listToSearch)
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
                closestDistanceSqr = dSqrtToTarget;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

}
