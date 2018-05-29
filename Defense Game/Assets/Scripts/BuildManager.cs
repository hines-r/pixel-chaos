using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    #region Singleton
    public static BuildManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManagere in scene!");
            return;
        }

        instance = this;
    }
    #endregion

    private UnitManager unitManager;

    private Node selectedNode;
    private Unit unitToPlace;

    void Start()
    {
        unitManager = UnitManager.instance;
    }

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            return;
        }

        if (selectedNode != null)
        {
            selectedNode.ToggleSeleted();
        }

        selectedNode = node;
        selectedNode.ToggleSeleted();
        unitToPlace = null;
    }

    public void DeselectNode()
    {
        if (selectedNode != null)
        {
            selectedNode.ToggleSeleted();
            selectedNode = null;
        }
    }

    public void SelectUnitToPlace(Unit unit)
    {
        // Checks if the unit is unlocked first
        if (unitManager.unlockedUnits.ContainsKey(unit.unitName))
        {
            Unit unitUnlocked = unitManager.unlockedUnits[unit.unitName];
            unitToPlace = unitUnlocked;
            return;
        }

        unitToPlace = unit;
    }

    public Unit GetUnitToPlace()
    {
        return unitToPlace;
    }

    public Node GetSelectedNode()
    {
        return selectedNode;
    }
}
