using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [Header("Unit Prefab")]
    public AttackingUnit unit;

    [Header("Image")]
    public Image unitImg;

    [Header("Name/Price")]
    public Text unitName;
    public Text priceTxt;

    [Header("Button Info")]
    public GameObject lockedArea;
    public Text buttonLevelText;

    internal bool isUnlocked;

    void Start()
    {
        if (unit != null)
        {
            unitImg.sprite = unit.unitSprite;
            unitName.text = unit.unitName;
            priceTxt.text = unit.baseCost + "g";
            UpdateLevelText();
        }
    }

    public void UpdateLevelText()
    {
        buttonLevelText.text = unit.level.ToString();
    }

    public void UnlockButton()
    {
        isUnlocked = true;
        lockedArea.SetActive(false);
        buttonLevelText.gameObject.SetActive(true);
    }

}
