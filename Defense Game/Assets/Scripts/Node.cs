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
    private UnitManager unitManager;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;

        buildManager = BuildManager.instance;
        unitManager = UnitManager.instance;
    }

    public void PlaceUnit(UnitBlueprint blueprint)
    {
        // Compares the names of the blueprint unit with any names within the unlocked units array
        // If there are any matches, place the unit within the unlocked unit array instead of
        // instantiating a new unit
        foreach (GameObject unlockedUnit in unitManager.unlockedUnits)
        {
            AttackingUnit storedUnit = unlockedUnit.GetComponent<AttackingUnit>();

            if (storedUnit != null)
            {
                AttackingUnit blueprintAttackingUnit = blueprint.prefab.GetComponent<AttackingUnit>();

                if (blueprintAttackingUnit != null)
                {
                    if (blueprintAttackingUnit.unitName == storedUnit.unitName)
                    {
                        storedUnit.gameObject.SetActive(true);
                        storedUnit.transform.position = transform.position;
                        storedUnit.currentNode.unit = null;
                        storedUnit.currentNode = this;

                        if (unit != null)
                        {
                            unit.SetActive(false);
                        }

                        unit = storedUnit.gameObject;
                        return;
                    }
                }
            }
        }

        GameObject _unit = Instantiate(blueprint.prefab, transform.position, Quaternion.identity) as GameObject;
        _unit.transform.parent = unitManager.transform;
        unit = _unit;

        AttackingUnit placedUnit = unit.GetComponent<AttackingUnit>();

        if (placedUnit != null)
        {
            placedUnit.currentNode = this; 
        }

        unitBlueprint = blueprint;
        unitManager.UnlockUnit(unit.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NodeUI ui = nodeUI.GetComponent<NodeUI>();

        ui.ShowSelectionPanel();

        buildManager.SelectNode(this);
    }
}
