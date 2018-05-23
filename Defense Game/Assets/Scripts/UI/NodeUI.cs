using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject unitSelectionUI;
    public GameObject unitPanelUI;
    public GameObject aiPanel;
    public GameObject aiPanelExtended;

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
    private AttackingUnit selectedStoredUnit;
    private bool unitIsStored;

    private Node target;
    private BuildManager buildManager;
    private UnitManager unitManager;
    private UnitButton[] buttons;

    void Start()
    {
        buildManager = BuildManager.instance;
        unitManager = UnitManager.instance;
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
            foreach (GameObject unlockedUnit in unitManager.unlockedUnits)
            {
                AttackingUnit storedUnit = unlockedUnit.GetComponent<AttackingUnit>();

                if (selectedUnit.unitName == storedUnit.unitName)
                {
                    if (PlayerStats.Gold >= storedUnit.upgradeCost)
                    {
                        PlayerStats.Gold -= (int)storedUnit.upgradeCost;
                        storedUnit.Upgrade();
                        UpdateUnitPanel();
                        return;
                    }
                    else
                    {
                        StartCoroutine(DisplayNotEnoughGoldDialog());
                    }
                }
            }
        }
    }

    void UpdateUnitPanel()
    {
        selectedUnit = buildManager.GetUnitToPlace().prefab.GetComponent<AttackingUnit>();

        if (selectedUnit != null)
        {
            foreach (GameObject unlockedUnit in unitManager.unlockedUnits)
            {
                AttackingUnit storedUnit = unlockedUnit.GetComponent<AttackingUnit>();

                if (storedUnit != null)
                {
                    if (selectedUnit.unitName == storedUnit.unitName)
                    {
                        unitIsStored = true;
                        selectedStoredUnit = storedUnit;

                        UpdateAIPanel();
                        UpdateUnitPanelComponents(storedUnit);

                        purchaseBtn.SetActive(false);
                        equipUpgradeBtnGroup.SetActive(true);
                        unitPanelUI.SetActive(true);

                        UpdatePanelButton();
                        return;
                    }
                }
            }
        }

        unitIsStored = false;
        UpdateUnitPanelComponents(selectedUnit);
        UpdateAIPanel();

        purchaseBtnTxt.text = "Purchase\n" + selectedUnit.baseCost + "g";
        purchaseBtn.SetActive(true);
        equipUpgradeBtnGroup.SetActive(false);

        unitPanelUI.SetActive(true);
    }

    void UpdateUnitPanelComponents(AttackingUnit unit)
    {
        unitImg.sprite = unit.unitSprite;
        unitLevelTxt.text = "Level: " + unit.level;
        unitNameTxt.text = unit.unitName;
        damageTxt.text = "Damage: " + unit.damage;
        attackSpeedTxt.text = "Speed: " + unit.attackSpeed + "s";
        descrptionTxt.text = unit.description;
        upgradeBtnTxt.text = "Upgrade\n" + unit.upgradeCost + "g";
    }

    void UpdateAIPanel()
    {
        if (unitIsStored)
        {
            Projectile p = selectedUnit.projectile.GetComponent<Projectile>();

            if (p != null)
            {
                LinearProjectile linearProjectile = p.GetComponent<LinearProjectile>();

                if (linearProjectile != null && linearProjectile.hasDot)
                {
                    aiPanel.SetActive(false);
                    aiPanelExtended.SetActive(true);
                }
                else
                {
                    aiPanel.SetActive(true);
                    aiPanelExtended.SetActive(false);
                }
            }
        }
        else
        {
            aiPanel.SetActive(false);
            aiPanelExtended.SetActive(false);
        }
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

    public void RemoveUnit()
    {
        if (target.unit != null)
        {
            target.unit.SetActive(false);
            target.unit = null;
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

            // Checks each unit button and unlocks it according to the unit purchased
            foreach (UnitButton button in buttons)
            {
                if (!button.isUnlocked)
                {
                    if (button.unit.unitName == unitToPurchase.unitName)
                    {
                        button.UnlockButton();
                        break;
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

    // Displays the not enough gold dialog by first disabling it and then
    // setting it active again so the animation resets
    IEnumerator DisplayNotEnoughGoldDialog()
    {
        float timeToWait = 5f;

        notEnoughGoldDialog.SetActive(false);
        notEnoughGoldDialog.SetActive(true);

        yield return new WaitForSeconds(timeToWait);

        // After waiting a bit, check the alpha of the not enough gold dialog
        // If it is 0 (not visible), set it as unactive
        CanvasGroup canvasGroup = notEnoughGoldDialog.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            if (canvasGroup.alpha <= 0)
            {
                notEnoughGoldDialog.SetActive(false);
            }
        }
    }
}
