using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caltrops : ParabolicProjectile
{
    public GameObject triggerEffect;
    private readonly float timeOfTriggerEffect = 1.5f;

    void TriggerTrap()
    {
        if (isOnGround)
        {
            if (triggerEffect != null)
            {
                GameObject effect = Instantiate(triggerEffect, transform.position, Quaternion.identity);
                Destroy(effect, timeOfTriggerEffect);
            }

            Explode();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerTrap();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        TriggerTrap();
    }

}
