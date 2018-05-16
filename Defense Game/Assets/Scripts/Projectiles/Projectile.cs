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

}
