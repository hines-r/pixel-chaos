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

    public PathButton firstChoice;
    public PathButton secondChoice;

    private BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    void UpdateAwakenPanelInfo()
    {
        unitName.text = nodeUI.selectedStoredUnit.unitName;
        //levelReqTxt.text = "Level " + nodeUI.selectedStoredUnit.levelToAwaken;
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
        StartCoroutine(DisplayDialog());
        /*
        buildManager.SelectUnitToPlace(nodeUI.selectedStoredUnit.firstChoiceBP);
        nodeUI.UpdateUnitPanel();
        HideAwakenPanel();
        */
    }

    public void SelectOptionTwo()
    {
        StartCoroutine(DisplayDialog());
        /*
        buildManager.SelectUnitToPlace(nodeUI.selectedStoredUnit.secondChoiceBP);
        nodeUI.UpdateUnitPanel();
        HideAwakenPanel();
        */
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
        public Image awokenImage;
    }
}
