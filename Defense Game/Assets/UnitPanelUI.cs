using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelUI : MonoBehaviour
{
    public Text unitNameTxt;
    public Text unitLevelTxt;
    public Text damageTxt;
    public Text attackSpeedTxt;
    public Text descrptionTxt;
    public Text upgradeBtnTxt;
    public Text equipBtnTxt;
    public Text purchaseBtnTxt;
    public Image unitImg;
    public GameObject equipUpgradeBtnGroup;
    public GameObject purchaseBtn;
    public GameObject awakenBtn;

    private Node target;
    private BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void SetTarget(Node node)
    {
        target = node;
    }

    public void UpdatePanelInfo(Unit unit)
    {
        unitNameTxt.text = unit.unitName;
        unitLevelTxt.text = "Level: " + unit.level;
        damageTxt.text = "Damage: " + unit.damage;
        attackSpeedTxt.text = "Speed: " + unit.attackSpeed + "s";
        descrptionTxt.text = unit.description;
        upgradeBtnTxt.text = "Upgrade\n" + unit.upgradeCost + "g";
        unitImg.sprite = unit.unitSprite;
    }

    void UpdatePanelButton()
    {
        if (target.unit != null)
        {
            if (buildManager.GetUnitToPlace().unitName == target.unit.unitName)
            {
                equipBtnTxt.text = "Unequip";
                return;
            }
        }

        equipBtnTxt.text = "Equip";
    }
}
