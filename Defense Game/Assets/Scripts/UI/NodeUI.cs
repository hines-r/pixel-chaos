using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject unitSelectionUI;
    public GameObject unitPanelUI;

    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    [Header("Available Units")]
    public UnitBlueprint rockThrower;
    public UnitBlueprint spearThrower;
    public UnitBlueprint standardArcher;
    public UnitBlueprint rocketMan;
    public UnitBlueprint ninja;
    public UnitBlueprint lightningWizard;
    public UnitBlueprint explosionWizard;
    public UnitBlueprint iceWizard;
    public UnitBlueprint dartBlower;
    public UnitBlueprint voidSage;

    [Space]

    [Header("Unit Panel Info")]
    public Image unitImg;
    public Text unitNameTxt;
    public Text unitLevelTxt;
    public Text damageTxt;
    public Text attackSpeedTxt;
    public Text descrptionTxt;
    public Text upgradeBtnTxt;
    public Text equipBtnTxt;
    public GameObject equipUpgradeBtnGroup;
    public GameObject purchaseBtn;
    public Text purchaseBtnTxt;

    public GameObject notEnoughGoldDialog;

    private AttackingUnit selectedUnit;
    private AttackingUnit currentlyPlacedUnit;

    private Node target;
    private BuildManager buildManager;
    private List<Node> currentNodes;
    private UnitButton[] buttons;

    void Start()
    {
        buildManager = BuildManager.instance;
        buttons = FindObjectsOfType<UnitButton>();
    }

    public void SetTarget(Node _target)
    {
        target = _target;
    }

    public void UpgradeButton()
    {
        if (selectedUnit != null)
        {
            selectedUnit.Upgrade(); // Updates values on unit panel
            currentlyPlacedUnit.Upgrade(); // Upgrades selected unit
            UpdateUnitPanel();
        }
    }

    void UpdateUnitPanel()
    {
        selectedUnit = buildManager.GetUnitToPlace().prefab.GetComponent<AttackingUnit>();

        if (selectedUnit != null)
        {
            unitImg.sprite = selectedUnit.unitSprite;
            unitLevelTxt.text = "Level: " + selectedUnit.level;
            unitNameTxt.text = selectedUnit.unitName;
            damageTxt.text = "Damage: " + selectedUnit.damage;
            attackSpeedTxt.text = "Speed: " + selectedUnit.attackSpeed + "s";
            descrptionTxt.text = selectedUnit.description;
        }

        // Checks to see if the selected unit is unlocked and changes the panel accordingly
        foreach (AttackingUnit unlockedUnit in buildManager.unlockedUnits)
        {
            if (unlockedUnit.unitName == selectedUnit.unitName)
            {
                purchaseBtn.SetActive(false);
                equipUpgradeBtnGroup.SetActive(true);
                unitPanelUI.SetActive(true);
                UpdatePanelButton();
                return;
            }
        }

        purchaseBtnTxt.text = "Purchase\n" + selectedUnit.baseCost + "g";
        purchaseBtn.SetActive(true);
        equipUpgradeBtnGroup.SetActive(false);


        unitPanelUI.SetActive(true);
    }

    void SwapActiveButtons()
    {
        purchaseBtn.SetActive(!purchaseBtn.activeSelf);
        equipUpgradeBtnGroup.SetActive(!equipUpgradeBtnGroup.activeSelf);
    }

    void UpdatePanelButton()
    {
        if (target.unit != null)
        {
            currentlyPlacedUnit = target.unit.GetComponent<AttackingUnit>();

            if (selectedUnit.unitName == currentlyPlacedUnit.unitName)
            {
                equipBtnTxt.text = "Unequip";
                return;
            }
        }

        equipBtnTxt.text = "Equip";
    }

    public void HideUnitPanel()
    {
        unitPanelUI.SetActive(false);
    }

    public void HideSelectionPanel()
    {
        Animator anim = unitSelectionUI.GetComponent<Animator>();
        anim.SetBool("Slide", false);
    }

    public void ShowSelectionPanel()
    {
        Animator anim = unitSelectionUI.GetComponent<Animator>();
        anim.SetBool("Slide", true);
    }

    IEnumerator DisableComponent(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(!obj.activeSelf);
    }

    public void RemoveUnit()
    {
        if (target.unit != null)
        {
            Destroy(target.unit);
            equipBtnTxt.text = "Equip";
        }
        else
        {
            Debug.Log("No unit on the target node!");
        }
    }

    public void SelectRockThrower()
    {
        buildManager.SelectUnitToPlace(rockThrower);
        UpdateUnitPanel();
    }

    public void SelectSpearThrower()
    {
        buildManager.SelectUnitToPlace(spearThrower);
        UpdateUnitPanel();
    }

    public void SelectStandardArcher()
    {
        buildManager.SelectUnitToPlace(standardArcher);
        UpdateUnitPanel();
    }

    public void SelectRocketMan()
    {
        buildManager.SelectUnitToPlace(rocketMan);
        UpdateUnitPanel();
    }

    public void SelectNinja()
    {
        buildManager.SelectUnitToPlace(ninja);
        UpdateUnitPanel();
    }

    public void SelectLightningWizard()
    {
        buildManager.SelectUnitToPlace(lightningWizard);
        UpdateUnitPanel();
    }

    public void SelectExplosionWizard()
    {
        buildManager.SelectUnitToPlace(explosionWizard);
        UpdateUnitPanel();
    }

    public void SelectIceWizard()
    {
        buildManager.SelectUnitToPlace(iceWizard);
        UpdateUnitPanel();
    }

    public void SelectDartBlower()
    {
        buildManager.SelectUnitToPlace(dartBlower);
        UpdateUnitPanel();
    }

    public void SelectVoidSage()
    {
        buildManager.SelectUnitToPlace(voidSage);
        UpdateUnitPanel();
    }

    public void PlaceRemoveButton()
    {
        if (target.unit != null)
        {
            currentlyPlacedUnit = target.unit.GetComponent<AttackingUnit>();

            if (selectedUnit.unitName == currentlyPlacedUnit.unitName)
            {
                RemoveUnit();
                return;
            }
        }

        target.PlaceUnit(buildManager.GetUnitToPlace());
        HideUnitPanel();
        UpdatePanelButton();
    }

    public void PurchaseUnit()
    {
        AttackingUnit unitToPurchase = buildManager.GetUnitToPlace().prefab.GetComponent<AttackingUnit>();

        // First checks if the player has enough gold and if so, subtracts it from player gold count
        if (PlayerStats.Gold >= unitToPurchase.baseCost)
        {
            PlayerStats.Gold -= unitToPurchase.baseCost;
            buildManager.UnlockUnit(unitToPurchase); // Unlocks the unit so the player doesn't have to purchase it again

            // Checks each unit button and unlocks it according to the unit purchased
            foreach (UnitButton button in buttons)
            {
                if (!button.isUnlocked)
                {
                    if (button.unit.unitName == unitToPurchase.unitName)
                    {
                        button.UnlockButton();
                    }
                }
            }

            PlaceRemoveButton();
        }
        else
        {
            StartCoroutine(DisplayNotEnoughGoldDialog());
        }
    }

    // Displays the not enough gold dialog by first disabling it and then setting it active again
    // so the animation resets
    IEnumerator DisplayNotEnoughGoldDialog()
    {
        float timeToWait = 3f;

        notEnoughGoldDialog.SetActive(false);
        notEnoughGoldDialog.SetActive(true);

        yield return new WaitForSeconds(timeToWait);
    }
    

}
