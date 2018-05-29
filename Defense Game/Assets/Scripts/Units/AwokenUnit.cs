using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwokenUnit : Unit, IAwoken
{
    [Header("Standard Version of Unit")]
    public StandardUnit originalUnit;

    //private UnitManager unitManager;

    protected override void Start()
    {
        base.Start();
        //unitManager = UnitManager.instance;
        CloneOriginal(); // Carries over the stats of the unawakened unit
    }

    protected override void Update()
    {
        base.Update();
    }

    public void CloneOriginal()
    {
        //Unit unitToClone = unitManager.FindUnlockedUnit(originalUnit);
    }
}
