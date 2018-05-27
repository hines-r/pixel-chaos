using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caster : MonoBehaviour
{
    public Unit unit;
    public GameObject castBar;
    public Image castProgress;

    internal bool isUnitReady;

    void Update()
    {
        if (ProceduralSpawner.CurrentState == ProceduralSpawner.State.Spawning)
        {
            castBar.SetActive(true);
        }
        else
        {
            castBar.SetActive(false);
            return;
        }

        castProgress.fillAmount = unit.GetNextAttackTime() / unit.attackSpeed;
    }
}
