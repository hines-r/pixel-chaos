using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D), typeof(Animator))]
public class Node : MonoBehaviour, IPointerClickHandler
{
    public UnitSelectionUI selectionUI;

    private Unit unit;
    private bool isUnitOnNode;

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
            if (selectionUI.gameObject.activeSelf || selectionUI.gameObject.activeSelf)
            {
                selectionUI.HideSelectionPanel();
            }
        }
        else
        {
            bc2d.enabled = true;
        }
    }

    public void PlaceUnit(Unit unitToPlace)
    {
        if (unit != null)
        {
            RemoveUnit(unit);
        }

        if (unitManager.unlockedUnits.ContainsKey(unitToPlace.unitName))
        {
            unitToPlace.gameObject.SetActive(true);
            unitToPlace.transform.position = transform.position;

            if (unitToPlace.currentNode != null)
            {
                unitToPlace.currentNode.unit = null;
                unitToPlace.currentNode.isUnitOnNode = false;
            }

            unitToPlace.currentNode = this;
            unit = unitToPlace;
            isUnitOnNode = true;
            return;
        }

        if (unitManager.IsUnitAwoken(unitToPlace))
        {
            AwokenUnit awokenUnit = unitToPlace.gameObject.GetComponent<AwokenUnit>();

            if (unitManager.unlockedUnits.ContainsKey(awokenUnit.originalUnit.unitName))
            {
                Unit unitToRemove = unitManager.unlockedUnits[awokenUnit.originalUnit.unitName];
                RemoveUnit(unitToRemove);
            }
        }

        Unit newUnit = Instantiate(unitToPlace, transform.position, Quaternion.identity);
        newUnit.transform.parent = unitManager.transform;
        newUnit.currentNode = this;

        unitManager.UnlockUnit(newUnit);

        unit = newUnit;
        isUnitOnNode = true;
    }

    public void RemoveUnit(Unit unitToRemove)
    {
        unitToRemove.currentNode = null;
        unitToRemove.gameObject.SetActive(false);

        unit = null;
        isUnitOnNode = false;
    }

    public bool IsUnitOnNode()
    {
        return isUnitOnNode;
    }

    public Unit GetUnitOnNode()
    {
        return unit;
    }

    public void ToggleSeleted()
    {
        anim.SetBool("isSelected", !anim.GetBool("isSelected"));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectionUI.ShowSelectionPanel();
        buildManager.SelectNode(this);
    }
}
