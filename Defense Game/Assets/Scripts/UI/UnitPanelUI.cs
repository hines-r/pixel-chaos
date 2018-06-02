using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
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

    private Animator anim;

    private BuildManager buildManager;
    private UnitManager unitManager;

    void Awake()
    {
        buildManager = BuildManager.instance;
        unitManager = UnitManager.instance;
        anim = GetComponent<Animator>();
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
        selectionUI.UpdateButton(selectedUnit);
        UpdateEquipText();
    }

    public void PurchaseButton()
    {
        if (!unitManager.IsUnitAwoken(selectedUnit))
        {
            if (PlayerStats.Gold >= selectedUnit.baseCost)
            {
                PlayerStats.Gold -= selectedUnit.baseCost;

                BuyUnit();

                if (Tutorial.instance.IsTutorial)
                {
                    Tutorial.instance.TriggerPhaseFour();
                }
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

                BuyUnit();
            }
            else
            {
                dialog.DisplayDialog("NOT ENOUGH GEMS!");
            }
        }
    }

    void BuyUnit()
    {
        selectedNode.PlaceUnit(selectedUnit);

        if (unitManager.unlockedUnits.ContainsKey(selectedUnit.unitName))
        {
            selectedUnit = unitManager.unlockedUnits[selectedUnit.unitName];
        }

        selectionUI.UpdateButton(selectedUnit);
        selectionUI.UnlockButton();
        HideUnitPanel();
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
        if (Tutorial.instance.IsTutorial)
        {
            Tutorial.instance.TriggerPhaseThree();
        }

        gameObject.SetActive(true);
        anim.Play("UnitPanelEntry");
    }

    public void ShowUnitPanelReentry()
    {
        gameObject.SetActive(true);
        anim.Play("UnitPanelRe-entry");
    }

    public void HideUnitPanel()
    {
        if (Tutorial.instance.IsTutorial)
        {
            Tutorial.instance.TriggerPhaseThree();
        }

        gameObject.SetActive(false);
    }
}
