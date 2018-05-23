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

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
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
