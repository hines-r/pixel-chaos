using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitBlueprint
{
    public GameObject prefab;

    internal bool isUnlocked;

    public void Upgrade()
    {
        AttackingUnit unit = prefab.GetComponent<AttackingUnit>();
        unit.Upgrade();
    }
}
