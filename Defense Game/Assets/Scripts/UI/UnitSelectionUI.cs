using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UnitSelectionUI : MonoBehaviour
{
    public UnitPanelUI unitPanelUI;
    public UnitButton[] unitButtons;

    public Dictionary<string, UnitButton> buttons;
    private UnitButton buttonSelection;

    private Animator anim;

    private BuildManager buildManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        buildManager = BuildManager.instance;

        buttons = new Dictionary<string, UnitButton>();

        for (int i = 0; i < unitButtons.Length; i++)
        {
            int index = i;
            unitButtons[i].button.onClick.AddListener(() => OnButtonClick(index));

            buttons.Add(unitButtons[i].unit.unitName, unitButtons[i]);
        }
    }

    void OnButtonClick(int index)
    {
        buttonSelection = unitButtons[index];
        buildManager.SelectUnitToPlace(unitButtons[index].unit);
        unitPanelUI.ShowUnitPanel();
    }

    public void UpdateButton(Unit unitForButton)
    {
        if (buttons.ContainsKey(unitForButton.unitName))
        {
            buttons[unitForButton.unitName].UpdateLevelText(unitForButton);         
        }
        else
        {
            buttonSelection.UpdateButton(unitForButton);
        }
    }

    public void UnlockButton()
    {
        buttonSelection.UnlockButton();
    }

    public void HideSelectionPanel()
    {
        anim.SetBool("Slide", false);
    }

    public void ShowSelectionPanel()
    {
        anim.SetBool("Slide", true);
    }
}
