using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwokenUnit : Unit
{
    [Header("Standard Version of Unit")]
    public StandardUnit originalUnit;

    private UnitManager unitManager;

    void Awake()
    {
        unitManager = UnitManager.instance;
        CloneOriginal(); // Carries over the stats of the unawakened unit
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    void CloneOriginal()
    {
        if (unitManager != null)
        {
            Unit unitToClone = unitManager.FindUnlockedUnit(originalUnit);

            if (unitToClone != null)
            {
                level = unitToClone.level;
            }
        }
    }
}
