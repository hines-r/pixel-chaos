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

    public PathButton firstPath;
    public PathButton secondPath;

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

        if (standardSelection != null)
        {
            unitName.text = standardSelection.unitName;
            levelReqTxt.text = "Level " + standardSelection.levelToAwaken;

            if (standardSelection.firstChoice != null)
            {
                firstPath.nameText.text = standardSelection.firstChoice.unitName;
                firstPath.awokenSprite.sprite = standardSelection.firstChoice.unitSprite;
            }
            else
            {
                firstPath.nameText.text = "Coming Soon!";
                firstPath.awokenSprite.sprite = standardSelection.unitSprite;
            }

            if (standardSelection.secondChoice != null)
            {
                secondPath.nameText.text = standardSelection.secondChoice.unitName;
                secondPath.awokenSprite.sprite = standardSelection.secondChoice.unitSprite;
            }
            else
            {
                secondPath.nameText.text = "Coming Soon!";
                secondPath.awokenSprite.sprite = standardSelection.unitSprite;
            }
            
            return;
        }

        Debug.Log("Standard unit type is null");
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

    public void SelectOptionOne()
    {
        if (standardSelection != null)
        {
            awokenSelection = standardSelection.firstChoice;

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
    }

    public void SelectOptionTwo()
    {
        if (standardSelection != null)
        {
            awokenSelection = standardSelection.secondChoice;

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
        public Text nameText;
        public Image awokenSprite;
    }
}
