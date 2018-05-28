using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionUI : MonoBehaviour
{
    public UnitButton[] unitButtons;

    private BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;

        for (int i = 0; i < unitButtons.Length; i++)
        {
            int index = i;
            unitButtons[i].button.onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void OnButtonClick(int index)
    {
        buildManager.SelectUnitToPlace(unitButtons[index].unit);
    }
}
