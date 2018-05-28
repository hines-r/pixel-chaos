using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwakenPanel : MonoBehaviour
{
    public NodeUI nodeUI;
    public GameObject unitPanel;
    public GameObject dialog;
    public Text unitName;
    public Text levelReqTxt;

    public PathButton[] pathButtons;

    private StandardUnit standardSelection;
    private AwokenUnit awokenSelection;

    private BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    void UpdateAwakenPanelInfo()
    {
        standardSelection = nodeUI.selectedStoredUnit.GetComponent<StandardUnit>();

        if (standardSelection == null)
        {
            standardSelection = nodeUI.selectedStoredUnit.GetComponent<AwokenUnit>().originalUnit;
        }

        for (int i = 0; i < pathButtons.Length; i++)
        {
            int index = i; // Needed so the listener doesn't receive the last element in the loop
            pathButtons[i].button.onClick.AddListener(() => OnButtonClick(index));

            if (standardSelection.awokenUnits.Length > 0)
            {
                if (standardSelection.awokenUnits[i] != null)
                {
                    pathButtons[i].Unit = standardSelection.awokenUnits[i];
                    pathButtons[i].nameText.text = standardSelection.awokenUnits[i].unitName;
                    pathButtons[i].awokenSprite.sprite = standardSelection.awokenUnits[i].unitSprite;
                }
                else
                {
                    pathButtons[i].Unit = null;
                    pathButtons[i].nameText.text = "Coming Soon!";
                    pathButtons[i].awokenSprite.sprite = standardSelection.unitSprite;
                }
            }
        }
    }

    public void OnButtonClick(int index)
    {
        awokenSelection = pathButtons[index].Unit;

        if (awokenSelection != null)
        {
            buildManager.SelectUnitToPlace(awokenSelection);
            nodeUI.UpdateUnitPanel();
            HideAwakenPanel();
        }
        else
        {
            StartCoroutine(DisplayDialog());
        }
    }

    public void ShowAwakenPanel()
    {
        UpdateAwakenPanelInfo();
        unitPanel.SetActive(false);
        gameObject.SetActive(true);
    }

    public void HideAwakenPanel()
    {
        unitPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator DisplayDialog()
    {
        float timeToWait = 5f;

        dialog.SetActive(false);
        dialog.SetActive(true);

        yield return new WaitForSeconds(timeToWait);

        CanvasGroup canvasGroup = dialog.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            if (canvasGroup.alpha <= 0)
            {
                dialog.SetActive(false);
            }
        }
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
