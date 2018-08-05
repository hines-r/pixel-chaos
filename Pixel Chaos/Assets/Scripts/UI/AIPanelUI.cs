using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIPanelUI : MonoBehaviour
{
    public AIPanel aiPanel;
    public AIPanel aiPanelExtended;

    private Unit selectedUnit;

    private BuildManager buildManager;
    private UnitManager unitManager;

    void Awake()
    {
        buildManager = BuildManager.instance;
        unitManager = UnitManager.instance;
    }

    void OnEnable()
    {
        UpdateAIPanel(buildManager.GetUnitToPlace());
    }

    void UpdateAIPanel(Unit _selectedUnit)
    {
        selectedUnit = _selectedUnit;

        if (unitManager.unlockedUnits.ContainsKey(selectedUnit.unitName))
        {
            Attack unitAttack = selectedUnit.attackPrefab.GetComponent<Attack>();

            if (unitAttack != null)
            {
                Projectile unitProjectile = unitAttack.GetComponent<Projectile>();

                // If the unit selected is using a linear projectile with 
                // a DoT (damage over time), display the extended ai panel
                if (unitProjectile != null && unitProjectile.hasDot)
                {
                    aiPanel.panel.SetActive(false);
                    aiPanelExtended.panel.SetActive(true);
                }
                else
                {
                    aiPanel.panel.SetActive(true);
                    aiPanelExtended.panel.SetActive(false);
                }
            }
        }
        else
        {
            aiPanel.panel.SetActive(false);
            aiPanelExtended.panel.SetActive(false);
        }

        // Selects the currently active AI when the panel is displayed
        SelectCurrentAIToggle();
    }

    public void SetAINearest()
    {
        selectedUnit.unitAI = Unit.AIType.Nearest;
    }

    public void SetAIRandom()
    {
        selectedUnit.unitAI = Unit.AIType.Random;
    }

    public void SetAIDoT()
    {
        selectedUnit.unitAI = Unit.AIType.Dot;
    }

    // Used to set the toggle of the currently active unit AI
    void SelectCurrentAIToggle()
    {
        if (selectedUnit.unitAI == Unit.AIType.Nearest)
        {
            aiPanel.SelectToggle(0);
            aiPanelExtended.SelectToggle(0);
        }
        else if (selectedUnit.unitAI == Unit.AIType.Random)
        {
            aiPanel.SelectToggle(1);
            aiPanelExtended.SelectToggle(1);
        }
        else if (selectedUnit.unitAI == Unit.AIType.Dot)
        {
            aiPanelExtended.SelectToggle(2);
        }
    }

    [System.Serializable]
    public struct AIPanel
    {
        public GameObject panel;
        public ToggleGroup toggleGroup;

        public Toggle[] Toggles
        {
            get { return toggleGroup.GetComponentsInChildren<Toggle>(); }
        }

        public void SelectToggle(int id)
        {
            // First sets all other toggles to false to ensure only one toggle is selected
            // (I had a problem with multiple toggles being active when changing them through
            // the script despite them being within a toggle group)
            for (int i = 0; i < Toggles.Length; i++)
            {
                Toggles[i].isOn = false;
            }

            Toggles[id].isOn = true;
        }
    }
}
