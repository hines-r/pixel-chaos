using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStorm : Attack
{
    public Attack iceSpear;
    public float duration;
    public float frequency;

    public float xMin = -1.5f;
    public float xMax = 1.5f;

    private Vector3 initialTargetPosition;
    private Vector3 impactLocation;

    void Start()
    {
        Vector2 screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        float yOffset = 1f;
        transform.position = new Vector3(Target.transform.position.x, screenHalfSizeWorldUnits.y + yOffset);

        initialTargetPosition = Target.transform.position;

        StartCoroutine(CastStorm());

        Destroy(gameObject, duration);
    }

    IEnumerator CastStorm()
    {
        while(true)
        {
            Vector3 randomPos = new Vector3(initialTargetPosition.x + Random.Range(xMin, xMax), transform.position.y);
            Attack attack = Instantiate(iceSpear, randomPos, Quaternion.identity);

            attack.Damage = Damage;
            attack.Target = Target;
         
            yield return new WaitForSeconds(frequency);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector3(transform.position.x + xMin, transform.position.y), new Vector3(transform.position.x + xMax, transform.position.y));
    }
}
