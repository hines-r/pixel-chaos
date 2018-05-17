using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
    public Color hoverColor;
    public Color startColor;

    public GameObject nodeUI;

    internal GameObject unit;
    internal UnitBlueprint unitBlueprint;

    private SpriteRenderer rend;

    private BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;

        buildManager = BuildManager.instance;
    }

    public void PlaceUnit(UnitBlueprint blueprint)
    {
        AttackingUnit unitToPlace = blueprint.prefab.GetComponent<AttackingUnit>();

        // Checks to see if there are any units of the same type within the nodes
        foreach (Node node in buildManager.currentNodes)
        {
            if (node.unit != null)
            {
                AttackingUnit unit = node.unit.GetComponent<AttackingUnit>();

                if (unit.unitName == unitToPlace.unitName)
                {
                    Destroy(node.unit);
                }
            }    
        }

        if (unit != null)
        {
            Destroy(unit);
        }

        GameObject _unit = Instantiate(blueprint.prefab, transform.position, Quaternion.identity) as GameObject;
        unit = _unit;

        unitBlueprint = blueprint;

        buildManager.UpdateNodeList();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NodeUI ui = nodeUI.GetComponent<NodeUI>();

        ui.ShowSelectionPanel();

        buildManager.SelectNode(this);
    }
}
