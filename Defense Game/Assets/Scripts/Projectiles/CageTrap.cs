using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageTrap : Attack
{
    public Cage cage;
    public float fallSpeed = 5f;
    public float duration = 3f;
    public float distanceAboveTarget = 4f;
    public float distanceToActivate = 1f;

    private Enemy targetEnemy;
    private Vector3 calculatedImpactLocation;

    void Start()
    {
        if (Target != null)
        {
            targetEnemy = Target.GetComponent<Enemy>();

            Vector3 targetPos = Target.transform.position;
            transform.position = new Vector3(targetPos.x, targetPos.y + distanceAboveTarget, targetPos.z);

            calculatedImpactLocation = LeadTarget(targetEnemy, targetEnemy.transform.position);
            Vector3 spawnPos = new Vector3(calculatedImpactLocation.x, calculatedImpactLocation.y + distanceAboveTarget, calculatedImpactLocation.z);

            transform.position = spawnPos;
        }
    }

    void Update()
    {
        if (Target != null)
        {
            if (transform.position.y <= Target.transform.position.y + distanceToActivate)
            {
                DeployCage();
            }
        }

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }


    Vector3 LeadTarget(Enemy targetEnemy, Vector3 targetPos)
    {
        Vector3 targetVelocity = targetEnemy.GetVelocity();

        float a = fallSpeed * fallSpeed - Vector3.Dot(targetVelocity, targetVelocity);
        float b = -2 * Vector3.Dot(targetVelocity, (targetPos - transform.position));
        float c = Vector3.Dot(-(targetPos - transform.position), (targetPos - transform.position));

        // Calculates the total time to the target
        float time = (b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        // Gets the displacement in time of the target
        float displacementX = targetVelocity.x * time;
        float displacementY = targetVelocity.y * time;

        Vector3 displacement = new Vector3(displacementX, displacementY);
        Vector3 futurePosition = targetPos - displacement;

        // Aims at the targets stopping point on the x axis if the displacement exceeds it
        if (targetEnemy.currentState != Enemy.State.Attacking || futurePosition.x >= targetEnemy.stoppingPoint)
        {
            targetPos -= displacement;
        }
        else
        {
            targetPos.x = targetEnemy.stoppingPoint;
        }

        return targetPos;
    }

    void DeployCage()
    {
        Cage newCage = Instantiate(cage, transform.position, Quaternion.identity);
        newCage.Damage = Damage;

        if (Target != null)
        {
            newCage.Target = Target;
        }

        newCage.impactLocation = calculatedImpactLocation;
        newCage.fallSpeed = fallSpeed;
        newCage.duration = duration;
        Destroy(gameObject);
    }
}
