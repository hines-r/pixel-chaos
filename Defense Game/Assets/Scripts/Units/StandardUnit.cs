using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardUnit : Unit
{
    [Header("Awoken Unit Upgrades")]
    public AwokenUnit[] awokenUnits;
    public int levelToAwaken;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
