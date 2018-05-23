using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject unitSelectionUI;
    public GameObject unitPanelUI;
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public AIPanel aiPanel;
    public AIPanel aiPanelExtended;

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
                    // Compares the unit name of the unit blueprint to see if it is already unlocked
                    // If so, sets the values within the unit panel equal to the unlocked unit instead
                    // This allows the panel to display the units upgrades and AI properly
                    if (selectedUnit.unitName == storedUnit.unitName)
                    {
                        unitIsStored = true;
                        selectedStoredUnit = storedUnit;

                        UpdateAIPanel();
                        UpdateUnitPanelComponents(storedUnit);

                        // Displays the equip and upgrade buttons instead of the purchase button
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

        // Displays the purchase button instead of the equip and upgrade buttons
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
            Projectile p = selectedStoredUnit.projectile.GetComponent<Projectile>();

            if (p != null)
            {
                LinearProjectile linearProjectile = p.GetComponent<LinearProjectile>();

                // If the unit selected is using a linear projectile with 
                // a DoT (damage over time), display the extended ai panel
                if (linearProjectile != null && linearProjectile.hasDot)
                {
                    aiPanel.panel.SetActive(false);
                    aiPanelExtended.panel.SetActive(true);
                }
                else
                {
                    aiPanel.panel.SetActive(true);
                    aiPanelExtended.panel.SetActive(false);
                }
            }

            // Selects the currently active AI when the panel is displayed
            SelectCurrentAIToggle();
        }
        else
        {
            // Wont display the AI panel if the unit has not been purchased first
            aiPanel.panel.SetActive(false);
            aiPanelExtended.panel.SetActive(false);
        }
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
            else
            {
                target.unit.SetActive(false);
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

    public void SetAINearest()
    {
        selectedStoredUnit.unitAI = AttackingUnit.AIType.Nearest;
    }

    public void SetAIRandom()
    {
        selectedStoredUnit.unitAI = AttackingUnit.AIType.Random;
    }

    public void SetAIDoT()
    {
        selectedStoredUnit.unitAI = AttackingUnit.AIType.Dot;
    }

    // Used to set the toggle of the currently active unit AI
    void SelectCurrentAIToggle()
    {       
        if (selectedStoredUnit.unitAI == AttackingUnit.AIType.Nearest)
        {
            aiPanel.SelectToggle(0);
            aiPanelExtended.SelectToggle(0);
        }
        else if (selectedStoredUnit.unitAI == AttackingUnit.AIType.Random)
        {
            aiPanel.SelectToggle(1);
            aiPanelExtended.SelectToggle(1);
        }
        else if (selectedStoredUnit.unitAI == AttackingUnit.AIType.Dot)
        {
            aiPanelExtended.SelectToggle(2);
        }
    }

    [System.Serializable]
    public struct AIPanel
    {
        public GameObject panel;
        public ToggleGroup toggleGroup;

        public Toggle[] Toggles
        {
            get { return toggleGroup.GetComponentsInChildren<Toggle>(); }
        }

        public void SelectToggle(int id)
        {
            // First sets all other toggles to false to ensure only one toggle is selected
            // (I had a problem with multiple toggles being active when changing them through
            // the script despite them being within a toggle group)
            for (int i = 0; i < Toggles.Length; i++)
            {
                Toggles[i].isOn = false;
            }

            Toggles[id].isOn = true;
        }
    }
}
