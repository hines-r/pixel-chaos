using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwokenUnit : Unit
{
    [Header("Standard Version of Unit")]
    public Unit originalUnit;

    private UnitManager unitManager;

    protected override void Start()
    {
        base.Start();
        unitManager = UnitManager.instance;
        CloneOriginal(); // Carries over the stats of the unawakened unit
    }

    protected override void Update()
    {
        base.Update();
    }

    void CloneOriginal()
    {
        Unit unitToClone = unitManager.FindUnlockedUnit(originalUnit);

        if (unitToClone != null)
        {
            upgradeCost = unitToClone.upgradeCost;
        }

        if (damage > originalUnit.damage)
        {
            float difference = damage - originalUnit.damage;
            damage += difference;
        }

        for (int i = 0; i < level; i++)
        {
            damage += originalUnit.damageIncrement;
        }
    }
}
