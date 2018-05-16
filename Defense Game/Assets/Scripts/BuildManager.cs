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

    public NodeUI nodeUI;

    private Node selectedNode;
    private UnitBlueprint unitToPlace;

    internal List<AttackingUnit> unlockedUnits;
    internal List<Node> currentNodes;

    void Start()
    {
        unlockedUnits = new List<AttackingUnit>();
        currentNodes = new List<Node>();
    }

    public void UnlockUnit(AttackingUnit unitToUnlock)
    {
        if (!unlockedUnits.Contains(unitToUnlock))
        {
            unlockedUnits.Add(unitToUnlock);
        }
    }

    public void UpdateNodeList()
    {
        Node[] nodes = FindObjectsOfType<Node>();

        foreach (Node node in nodes)
        {
            currentNodes.Add(node);
        }
    }

    public void StoreUnitOnNode(Node node)
    {
        node = selectedNode;
    }

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            nodeUI.RemoveUnit();
            return;
        }

        if (selectedNode != null)
        {
            selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.startColor;
        }

        selectedNode = node;
        selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.hoverColor;
        unitToPlace = null;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        if (selectedNode != null)
        {
            selectedNode.GetComponent<SpriteRenderer>().color = selectedNode.startColor;
            selectedNode = null;
        }

        nodeUI.HideSelectionPanel();
    }

    public void SelectUnitToPlace(UnitBlueprint unit)
    {
        unitToPlace = unit;
    }

    public UnitBlueprint GetUnitToPlace()
    {
        return unitToPlace;
    }
}
