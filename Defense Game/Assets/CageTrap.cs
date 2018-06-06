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

    private Vector3 impactLocation;
    private Cage newCage;

    void Start()
    {
        Vector3 targetPos = Target.transform.position;
        Vector3 spawnPos = new Vector3(targetPos.x, targetPos.y + distanceAboveTarget, targetPos.z);
        transform.position = spawnPos;

        impactLocation = Target.transform.position;
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
        else
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
    }

    void DeployCage()
    {
        newCage = Instantiate(cage, transform.position, Quaternion.identity);
        newCage.Target = Target;
        newCage.fallSpeed = fallSpeed;
        newCage.duration = duration;
        Destroy(gameObject);
    }
}
