using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region Singleton
    public static UnitManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one UnitManager in scene!");
            return;
        }

        instance = this;
    }
    #endregion

    public List<Unit> unlockedUnits;

    void Start()
    {
        unlockedUnits = new List<Unit>();
    }

    public void UnlockUnit(Unit unitToUnlock)
    {
        if (!unlockedUnits.Contains(unitToUnlock))
        {
            unlockedUnits.Add(unitToUnlock);
        }
    }

    public Unit FindUnlockedUnit(Unit unitToCompare)
    {
        if (unlockedUnits.Contains(unitToCompare))
        {
            print("this works!!");
        }

        foreach (Unit unit in unlockedUnits)
        {
            if (unit != null)
            {
                if (unitToCompare.unitName == unit.unitName)
                {
                    return unit;
                }
            }
        }

        return null;
    }
}
