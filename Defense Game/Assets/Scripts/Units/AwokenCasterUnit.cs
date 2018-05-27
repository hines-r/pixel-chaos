using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwokenCasterUnit : CasterUnit, IAwoken
{
    [Header("Standard Version of Unit")]
    public Unit originalUnit;

    private UnitManager unitManager;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void CloneOriginal()
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

        for (int i = 1; i < level; i++)
        {
            damage += originalUnit.damageIncrement;
        }
    }
}
