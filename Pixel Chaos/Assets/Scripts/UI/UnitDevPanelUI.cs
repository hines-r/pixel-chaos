using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnitDevPanelUI : MonoBehaviour
{
    // UI panels
    public UnitPanelUI unitPanel;
    public UnitSelectionUI unitSelectionUI;

    // Input fields
    public InputField levelInput;
    public InputField damageInput;
    public InputField speedInput;

    private BuildManager bm;
    private UnitManager um;
    private Unit selectedUnit;

    void Awake()
    {
        bm = BuildManager.instance;
        um = UnitManager.instance;
    }

    void OnEnable()
    {
        UpdateInputFields();
    }

    void UpdateInputFields()
    {
        selectedUnit = bm.GetUnitToPlace();

        levelInput.text = selectedUnit.level.ToString();
        damageInput.text = selectedUnit.damage.ToString();
        speedInput.text = selectedUnit.attackSpeed.ToString();
    }

    public void SetLevel()
    {
        selectedUnit = bm.GetUnitToPlace();

        if (um.unlockedUnits.ContainsKey(selectedUnit.unitName))
        {
            selectedUnit = um.unlockedUnits[selectedUnit.unitName];
        }

        int level;
        if (Int32.TryParse(levelInput.text, out level))
        {
            selectedUnit.level = level;
        }

        unitPanel.UpdateUnitStats();
        unitSelectionUI.buttons[selectedUnit.unitName].UpdateButton(selectedUnit);
    }

    public void SetDamage()
    {
        selectedUnit = bm.GetUnitToPlace();

        if (um.unlockedUnits.ContainsKey(selectedUnit.unitName))
        {
            selectedUnit = um.unlockedUnits[selectedUnit.unitName];
        }

        int damage;
        if (Int32.TryParse(damageInput.text, out damage))
        {
            selectedUnit.damage = damage;
        }

        unitPanel.UpdateUnitStats();
        unitSelectionUI.buttons[selectedUnit.unitName].UpdateButton(selectedUnit);
    }

    public void SetSpeed()
    {
        selectedUnit = bm.GetUnitToPlace();

        if (um.unlockedUnits.ContainsKey(selectedUnit.unitName))
        {
            selectedUnit = um.unlockedUnits[selectedUnit.unitName];
        }

        float speed;
        if (float.TryParse(speedInput.text, out speed))
        {
            selectedUnit.attackSpeed = speed;
        }

        unitPanel.UpdateUnitStats();
        unitSelectionUI.buttons[selectedUnit.unitName].UpdateButton(selectedUnit);
    }
}
