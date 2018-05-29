using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwakenPanelUI : MonoBehaviour
{
    public UnitPanelUI unitPanelUI;
    public DialogUI dialog;
    public Text unitName;
    public Text levelReqTxt;

    public PathButton[] pathButtons;

    private Unit selectedUnit;
    private StandardUnit standardUnit;
    private AwokenUnit awokenUnit;

    private BuildManager buildManager;

    void Awake()
    {
        buildManager = BuildManager.instance;
    }

    void OnEnable()
    {
        selectedUnit = buildManager.GetUnitToPlace();

        standardUnit = selectedUnit.gameObject.GetComponent<StandardUnit>();

        UpdateAwakenPanelInfo();
    }

    void UpdateAwakenPanelInfo()
    {
        if (standardUnit == null)
        {
            standardUnit = selectedUnit.GetComponent<AwokenUnit>().originalUnit;
        }

        if (standardUnit != null)
        {
            for (int i = 0; i < pathButtons.Length; i++)
            {
                int index = i; // Needed so the listener doesn't receive the last element in the loop
                pathButtons[i].button.onClick.AddListener(() => OnButtonClick(index));

                if (standardUnit.awokenUnits.Length > 0)
                {
                    if (standardUnit.awokenUnits[i] != null)
                    {
                        pathButtons[i].Unit = standardUnit.awokenUnits[i];
                        pathButtons[i].nameText.text = standardUnit.awokenUnits[i].unitName;
                        pathButtons[i].awokenSprite.sprite = standardUnit.awokenUnits[i].unitSprite;
                    }
                    else
                    {
                        pathButtons[i].Unit = null;
                        pathButtons[i].nameText.text = "Coming Soon!";
                        pathButtons[i].awokenSprite.sprite = standardUnit.unitSprite;
                    }
                }
            }
        }
    }

    public void OnButtonClick(int index)
    {
        awokenUnit = pathButtons[index].Unit;

        if (awokenUnit != null)
        {
            buildManager.SelectUnitToPlace(awokenUnit);
            HideAwakenPanel();
        }
        else
        {
            dialog.DisplayDialog("COMING SOON");
        }
    }

    public void ShowAwakenPanel()
    {
        unitPanelUI.HideUnitPanel();
        gameObject.SetActive(true);
    }

    public void HideAwakenPanel()
    {
        unitPanelUI.ShowUnitPanelReentry();
        gameObject.SetActive(false);
    }

    [System.Serializable]
    public struct PathButton
    {
        public Button button;
        public Text nameText;
        public Image awokenSprite;

        public AwokenUnit Unit { get; set; }
    }
}
