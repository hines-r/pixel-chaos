using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShell : LinearProjectile
{
    public int burstCount = 10;

    [Range(0f, 2f)]
    public float scatterOffset = 0.5f;

    public GameObject muzzleFlash;
    private readonly float flashTime = 1.5f;

    // Modifier used to slightly vary the projectile speed
    private readonly float minSpeedOffset = 0.95f;
    private readonly float maxSpeedOffset = 1.15f;

    protected override void Start()
    {
        base.Start();

        if (muzzleFlash != null)
        {
            GameObject flashEffect = Instantiate(muzzleFlash, transform.position, transform.rotation);
            Destroy(flashEffect, flashTime);
        }

        for (int i = 0; i < burstCount; i++)
        {
            ShotgunShell newProjectile = Instantiate(this, transform.position, Quaternion.identity);
            newProjectile.Damage = Damage;
            newProjectile.Target = Target;
            newProjectile.speed = speed * Random.Range(minSpeedOffset, maxSpeedOffset);
            newProjectile.muzzleFlash = null;
            newProjectile.burstCount = 0;
        }
    }

    protected override void FaceTarget()
    {
        Vector3 targetPos = Target.transform.position;
        Vector3 scatterPos = new Vector3(targetPos.x + Random.Range(-scatterOffset, scatterOffset), 
            targetPos.y + Random.Range(-scatterOffset, scatterOffset), targetPos.z);

        transform.right = scatterPos - transform.position;
    }
}
