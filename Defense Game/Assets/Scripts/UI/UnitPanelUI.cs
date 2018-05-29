using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelUI : MonoBehaviour
{
    [Header("Panel Info")]
    public UnitSelectionUI selectionUI;
    public AIPanelUI aiPanelUI;

    // Top panel
    public Text unitNameTxt;

    // Left Panel
    public Image unitImg;
    public Text unitLevelTxt;
    public Text damageTxt;
    public Text attackSpeedTxt;

    // Right Panel
    public Text descrptionTxt;
    public Text upgradeBtnTxt;
    public Text equipBtnTxt;
    public Text purchaseBtnTxt;
    public GameObject equipUpgradeBtnGroup;
    public GameObject purchaseBtn;
    public GameObject awakenBtn;

    [Header("Dialog Box")]
    public DialogUI dialog;

    private Unit selectedUnit;
    private Node selectedNode;

    private BuildManager buildManager;
    private UnitManager unitManager;

    void Awake()
    {
        buildManager = BuildManager.instance;
        unitManager = UnitManager.instance;
    }

    void OnEnable()
    {
        selectedUnit = buildManager.GetUnitToPlace();
        selectedNode = buildManager.GetSelectedNode();

        UpdatePanelInfo();
    }

    void UpdatePanelInfo()
    {
        unitNameTxt.text = selectedUnit.unitName;
        unitImg.sprite = selectedUnit.unitSprite;
        descrptionTxt.text = selectedUnit.description;

        UpdateUnitStats();
        DisplayCorrectButtons();
    }

    void UpdateUnitStats()
    {
        unitLevelTxt.text = "Level: " + selectedUnit.level;
        damageTxt.text = "Damage: " + selectedUnit.damage;
        attackSpeedTxt.text = "Speed: " + selectedUnit.attackSpeed + "s";
        upgradeBtnTxt.text = "Upgrade\n" + selectedUnit.upgradeCost + "g";
    }

    void DisplayCorrectButtons()
    {
        if (unitManager.unlockedUnits.ContainsKey(selectedUnit.unitName))
        {
            equipUpgradeBtnGroup.SetActive(true);
            purchaseBtn.SetActive(false);
            awakenBtn.SetActive(true);

            UpdateEquipText();
        }
        else
        {
            equipUpgradeBtnGroup.SetActive(false);
            purchaseBtn.SetActive(true);
            awakenBtn.SetActive(false);

            UpdatePurchaseText();
        }
    }

    void UpdatePurchaseText()
    {
        if (!unitManager.IsUnitAwoken(selectedUnit))
        {
            purchaseBtnTxt.text = "Purchase\n" + selectedUnit.baseCost + "g";
        }
        else
        {
            purchaseBtnTxt.text = "Purchase\n" + selectedUnit.baseCost + " Gems";
        }
    }

    void UpdateEquipText()
    {
        if (selectedNode.IsUnitOnNode())
        {
            if (selectedNode.GetUnitOnNode().unitName == selectedUnit.unitName)
            {
                equipBtnTxt.text = "Unequip";
                return;
            }
        }

        equipBtnTxt.text = "Equip";
    }

    public void EquipButton()
    {
        if (selectedNode.IsUnitOnNode())
        {
            if (selectedNode.GetUnitOnNode().unitName == selectedUnit.unitName)
            {
                selectedNode.RemoveUnit(selectedNode.GetUnitOnNode());
                UpdateEquipText();
                return;
            }
        }

        selectedNode.PlaceUnit(selectedUnit);
        UpdateEquipText();
    }

    public void PurchaseButton()
    {
        if (!unitManager.IsUnitAwoken(selectedUnit))
        {
            if (PlayerStats.Gold >= selectedUnit.baseCost)
            {
                PlayerStats.Gold -= selectedUnit.baseCost;

                selectedNode.PlaceUnit(selectedUnit);
                selectionUI.UpdateButton(selectedUnit);
                selectionUI.UnlockButton();
                HideUnitPanel();
            }
            else
            {
                dialog.DisplayDialog("NOT ENOUGH GOLD!");
            }
        }
        else
        {
            if (PlayerStats.Gems >= selectedUnit.baseCost)
            {
                PlayerStats.Gems -= selectedUnit.baseCost;

                selectedNode.PlaceUnit(selectedUnit);
                selectionUI.UpdateButton(selectedUnit);
                selectionUI.UnlockButton();
                HideUnitPanel();
            }
            else
            {
                dialog.DisplayDialog("NOT ENOUGH GEMS!");
            }
        }
    }

    public void UpgradeButton()
    {
        if (PlayerStats.Gold >= (int)selectedUnit.upgradeCost)
        {
            PlayerStats.Gold -= (int)selectedUnit.upgradeCost;

            selectedUnit.Upgrade();
            selectionUI.UpdateButton(selectedUnit);
            UpdateUnitStats();
        }
        else
        {
            dialog.DisplayDialog("NOT ENOUGH GOLD!");
        }
    }

    public void ShowUnitPanel()
    {
        gameObject.SetActive(true);
    }

    public void HideUnitPanel()
    {
        gameObject.SetActive(false);
    }
}
