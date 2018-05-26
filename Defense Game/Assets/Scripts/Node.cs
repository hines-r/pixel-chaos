using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IPointerClickHandler
{
    public Color selectedColor;
    public Color startColor;

    public GameObject nodeUI;

    internal Unit unit;

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

    public void PlaceUnit(Unit unitToPlace)
    {
        // Compares the names of the blueprint unit with any names within the unlocked units array
        // If there are any matches, place the unit within the unlocked unit array instead of
        // instantiating a new unit
        foreach (Unit unlockedUnit in unitManager.unlockedUnits)
        {
            if (unlockedUnit != null)
            {
                if (unitToPlace != null)
                {
                    if (unitToPlace.unitName == unlockedUnit.unitName)
                    {
                        unlockedUnit.gameObject.SetActive(true);
                        unlockedUnit.transform.position = transform.position;
                        unlockedUnit.currentNode.unit = null;
                        unlockedUnit.currentNode = this;

                        if (unit != null)
                        {
                            unit.gameObject.SetActive(false);
                        }

                        unit = unlockedUnit;
                        return;
                    }
                }
            }
        }

        Unit _unit = Instantiate(unitToPlace, transform.position, Quaternion.identity);
        _unit.transform.parent = unitManager.transform;
        unit = _unit;

        unit.currentNode = this;
        unitManager.UnlockUnit(unit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NodeUI ui = nodeUI.GetComponent<NodeUI>();

        ui.ShowSelectionPanel();

        buildManager.SelectNode(this);
    }
}
