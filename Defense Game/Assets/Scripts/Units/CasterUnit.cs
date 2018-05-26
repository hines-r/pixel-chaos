using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasterUnit : StandardUnit
{
    [Header("Cast Bar")]
    public GameObject castBar;
    public Image castProgress;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (ProceduralSpawner.CurrentState == ProceduralSpawner.State.Spawning)
        {
            castBar.SetActive(true);
        }
        else
        {
            castBar.SetActive(false);
            return;
        }

        castProgress.fillAmount = nextAttackTime / attackSpeed;
    }
}
