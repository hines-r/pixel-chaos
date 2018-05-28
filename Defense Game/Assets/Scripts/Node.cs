using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D), typeof(Animator))]
public class Node : MonoBehaviour, IPointerClickHandler
{
    public NodeUI ui;

    internal Unit unit;

    private BoxCollider2D bc2d;
    private Animator anim;

    private BuildManager buildManager;
    private UnitManager unitManager;

    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        buildManager = BuildManager.instance;
        unitManager = UnitManager.instance;
    }

    void Update()
    {
        if (ProceduralSpawner.CurrentState != ProceduralSpawner.State.Waiting)
        {
            bc2d.enabled = false;
            if (ui.unitSelectionUI.activeSelf || ui.unitPanelUI.activeSelf)
            {
                ui.HideSelectionPanel();
                ui.HideUnitPanel();
            }
        }
        else
        {
            bc2d.enabled = true;
        }
    }

    public void PlaceUnit(Unit unitToPlace)
    {
        Unit unlockedUnit = unitManager.FindUnlockedUnit(unitToPlace);

        if(unlockedUnit != null)
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

        Unit _unit = Instantiate(unitToPlace, transform.position, Quaternion.identity);
        _unit.transform.parent = unitManager.transform;
        unit = _unit;

        unit.currentNode = this;
        unitManager.UnlockUnit(unit);
    }

    public void ToggleSeleted()
    {
        anim.SetBool("isSelected", !anim.GetBool("isSelected"));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ui.ShowSelectionPanel();
        buildManager.SelectNode(this);
    }
}
