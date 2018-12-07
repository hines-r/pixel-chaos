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

    public Dictionary<string, Unit> unlockedUnits;
    public Node[] nodes;

    void Start()
    {
        unlockedUnits = new Dictionary<string, Unit>();
    }

    public void UnlockUnit(Unit unitToUnlock)
    {
        if (!unlockedUnits.ContainsKey(unitToUnlock.unitName))
        {
            unlockedUnits.Add(unitToUnlock.unitName, unitToUnlock);
        }
    }

    public Unit FindUnlockedUnit(Unit unitToCompare)
    { 
        if (unlockedUnits.ContainsKey(unitToCompare.unitName))
        {
            return unlockedUnits[unitToCompare.unitName];
        }

        return null;
    }

    public bool IsUnitAwoken(Unit unit)
    {
        if (unit.GetComponent<AwokenUnit>() != null)
        {
            return true;
        }

        return false;
    }

    public List<AwokenUnit> FindUnlockedSiblings(AwokenUnit unitToSearch)
    {
        List<AwokenUnit> siblings = new List<AwokenUnit>();

        for (int i = 0; i < unitToSearch.originalUnit.awokenUnits.Length; i++)
        {
            AwokenUnit sibling = unitToSearch.originalUnit.awokenUnits[i];

            if (sibling != null)
            {
                if (unitToSearch.unitName != sibling.unitName)
                {
                    if (unlockedUnits.ContainsKey(sibling.unitName))
                    {
                        siblings.Add(unlockedUnits[sibling.unitName] as AwokenUnit);
                    }
                }
            }
        }

        return siblings;
    }

    public void DisableSiblings(AwokenUnit unitToFindSiblings)
    {
        foreach (AwokenUnit unit in FindUnlockedSiblings(unitToFindSiblings))
        {
            unit.gameObject.SetActive(false);
        }
    }

    public void RemoveUnit(Unit unitToRemove)
    {
        if (unlockedUnits.ContainsKey(unitToRemove.unitName))
        {
            unlockedUnits.Remove(unitToRemove.unitName);
            Destroy(unitToRemove.gameObject);
        }
    }

    public List<AwokenUnit> GetAwokenUnits()
    {
        List<AwokenUnit> awokenUnits = new List<AwokenUnit>();
        foreach (AwokenUnit awokenUnit in unlockedUnits.Values)
        {
            awokenUnits.Add(awokenUnit);
        }

        return awokenUnits;
    }

    public List<StandardUnit> GetStandardUnits()
    {
        List<StandardUnit> standardUnits = new List<StandardUnit>();
        foreach (StandardUnit standardUnit in unlockedUnits.Values)
        {
            standardUnits.Add(standardUnit);
        }

        return standardUnits;
    }
}
