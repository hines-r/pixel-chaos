using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    /*
    public GameObject unitSelectionUI;
    public GameObject unitPanelUI;
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public AIPanel aiPanel;
    public AIPanel aiPanelExtended;

    [Header("Available Units")]
    public UnitButton rockThrower;
    public UnitButton spearThrower;
    public UnitButton standardArcher;
    public UnitButton ninja;
    public UnitButton rocketMan;
    public UnitButton lightningWizard;
    public UnitButton iceWizard;
    public UnitButton dartBlower;
    public UnitButton explosionWizard;
    public UnitButton voidSage;

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
    public Text purchaseBtnTxt;
    public GameObject equipUpgradeBtnGroup;
    public GameObject purchaseBtn;
    public GameObject awakenBtn;

    public GameObject notEnoughGoldDialog;
    public GameObject notEnoughGemsDialog;

    internal Unit selectedUnit;
    private Unit currentlyPlacedUnit;
    
    internal Unit selectedStoredUnit;
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
            Unit unlockedUnit = unitManager.FindUnlockedUnit(selectedUnit);

            if (unlockedUnit != null)
            {
                if (PlayerStats.Gold >= unlockedUnit.upgradeCost)
                {
                    PlayerStats.Gold -= (int)unlockedUnit.upgradeCost;
                    unlockedUnit.Upgrade();
                    UpdateUnitButtonLevel();
                    UpdateUnitPanel();
                    return;
                }
                else
                {
                    StartCoroutine(DisplayDialog(notEnoughGoldDialog));
                }
            }
        }
    }

    void UpdateUnitButtonLevel()
    {
        foreach (UnitButton button in buttons)
        {
            if (button.isUnlocked)
            {
                if (button.unit.unitName == selectedStoredUnit.unitName)
                {
                    button.UpdateLevelText(selectedStoredUnit);
                    break;
                }
            }
        }
    }

    public void UpdateUnitPanel()
    {
        selectedUnit = buildManager.GetUnitToPlace();

        if (selectedUnit != null)
        {
            // Compares the selected unit with units within the unlocked units array for a match
            // If a match is found, sets the values within the unit panel equal to the unlocked unit instead
            // This allows the panel to display the units upgrades and AI properly          
            Unit unlockedUnit = unitManager.FindUnlockedUnit(selectedUnit);

            if (unlockedUnit != null)
            {
                unitIsStored = true;
                selectedStoredUnit = unlockedUnit;

                UpdateAIPanel();
                UpdateUnitPanelComponents(unlockedUnit);

                // Displays the equip and upgrade buttons instead of the purchase button
                purchaseBtn.SetActive(false);
                equipUpgradeBtnGroup.SetActive(true);

                AwokenUnit awoken = unlockedUnit.GetComponent<AwokenUnit>();
                
                if (awoken != null)
                {
                    awakenBtn.SetActive(false);
                }
                else
                {
                    awakenBtn.SetActive(true);
                }

                unitPanelUI.SetActive(true);
                UpdatePanelButton();
            }
            else
            {
                unitIsStored = false;
                UpdateUnitPanelComponents(selectedUnit);
                UpdateAIPanel();

                AwokenUnit awokenUnit = selectedUnit.GetComponent<AwokenUnit>();

                if (awokenUnit != null)
                {
                    purchaseBtnTxt.text = "Purchase\n" + awokenUnit.baseCost + " Gems";
                }
                else
                {
                    purchaseBtnTxt.text = "Purchase\n" + selectedUnit.baseCost + "g";
                }

                // Displays the purchase button instead of the equip and upgrade buttons             
                purchaseBtn.SetActive(true);
                equipUpgradeBtnGroup.SetActive(false);

                awakenBtn.SetActive(false);

                unitPanelUI.SetActive(true);
            }
        }
    }

    void UpdateUnitPanelComponents(Unit unit)
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
            Attack unitAttack = selectedStoredUnit.attackPrefab.GetComponent<Attack>();

            if (unitAttack != null)
            {
                LinearProjectile linearProjectile = unitAttack.GetComponent<LinearProjectile>();

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
            currentlyPlacedUnit = target.unit.GetComponent<Unit>();

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

    public void RemoveUnit(Unit unitToRemove)
    {
        if (unitToRemove != null)
        {
            unitToRemove.gameObject.SetActive(false);
            unitToRemove = null;
            equipBtnTxt.text = "Equip";
        }
        else
        {
            Debug.Log("No unit on the target node!");
        }
    }

    public void SelectRockThrower()
    {  
        buildManager.SelectUnitToPlace(rockThrower.unit);
        UpdateUnitPanel();
    }

    public void SelectSpearThrower()
    {
        buildManager.SelectUnitToPlace(spearThrower.unit);
        UpdateUnitPanel();
    }

    public void SelectStandardArcher()
    {
        buildManager.SelectUnitToPlace(standardArcher.unit);
        UpdateUnitPanel();
    }

    public void SelectRocketMan()
    {
        buildManager.SelectUnitToPlace(rocketMan.unit);
        UpdateUnitPanel();
    }

    public void SelectNinja()
    {
        buildManager.SelectUnitToPlace(ninja.unit);
        UpdateUnitPanel();
    }

    public void SelectLightningWizard()
    {
        buildManager.SelectUnitToPlace(lightningWizard.unit);
        UpdateUnitPanel();
    }

    public void SelectExplosionWizard()
    {
        buildManager.SelectUnitToPlace(explosionWizard.unit);
        UpdateUnitPanel();
    }

    public void SelectIceWizard()
    {
        buildManager.SelectUnitToPlace(iceWizard.unit);
        UpdateUnitPanel();
    }

    public void SelectDartBlower()
    {
        buildManager.SelectUnitToPlace(dartBlower.unit);
        UpdateUnitPanel();
    }

    public void SelectVoidSage()
    {
        buildManager.SelectUnitToPlace(voidSage.unit);
        UpdateUnitPanel();
    }

    public void PlaceRemoveButton()
    {
        if (target.unit != null)
        {
            currentlyPlacedUnit = target.unit.GetComponent<Unit>();

            if (selectedUnit.unitName == currentlyPlacedUnit.unitName)
            {
                RemoveUnit(target.unit);
                return;
            }
            else
            {
                target.unit.gameObject.SetActive(false);
            }
        }

        target.PlaceUnit(buildManager.GetUnitToPlace());
        HideUnitPanel();
        UpdatePanelButton();
    }

    public void PurchaseUnit()
    {
        Unit unitToPurchase = buildManager.GetUnitToPlace();
        AwokenUnit awokenUnit = unitToPurchase.GetComponent<AwokenUnit>();

        // First checks if the unit to purchase is an awoken unit to utilize gems
        if (awokenUnit != null)
        {
            if (PlayerStats.Gems >= awokenUnit.baseCost)
            {
                PlayerStats.Gems -= awokenUnit.baseCost;

                UnitButton button = FindUnitButton(awokenUnit.originalUnit);

                if (button != null)
                {
                    button.UpdateButton(awokenUnit);

                    Unit standardVersion = unitManager.FindUnlockedUnit(awokenUnit.originalUnit);

                    if (standardVersion != null)
                    {
                        // Removes standard unit from any current node if purchasing
                        // the awoken version from a different selected node
                        standardVersion.currentNode.unit = null;
                        Destroy(standardVersion.gameObject);
                    }

                    PlaceRemoveButton();
                    return;
                }

                foreach(AwokenUnit sibling in unitManager.FindUnlockedSiblings(awokenUnit))
                {
                    UnitButton awokenUnitButton = FindUnitButton(sibling);

                    if (awokenUnitButton != null)
                    {
                        awokenUnitButton.UpdateButton(awokenUnit);
                        sibling.currentNode.unit = null;
                        sibling.gameObject.SetActive(false);
                    }
                }
            }
        }

        // If not awoken, the unit is standard and requires gold
        if (PlayerStats.Gold >= unitToPurchase.baseCost)
        {
            PlayerStats.Gold -= unitToPurchase.baseCost;

            UnitButton button = FindUnitButton(unitToPurchase);

            if (button != null)
            {
                button.UnlockButton();
            }

            PlaceRemoveButton();
        }
        else
        {
            StartCoroutine(DisplayDialog(notEnoughGoldDialog));
        }
    }

    UnitButton FindUnitButton(Unit unitToSearch)
    {
        foreach (UnitButton button in buttons)
        {
            if (button.unit.unitName == unitToSearch.unitName)
            {
                return button;
            }
        }

        return null;
    }

    // Displays a dialog by first disabling it and then
    // setting it active again so the animation resets
    IEnumerator DisplayDialog(GameObject dialog)
    {
        float timeToWait = 5f;

        dialog.SetActive(false);
        dialog.SetActive(true);

        yield return new WaitForSeconds(timeToWait);

        // After waiting a bit, check the alpha of the not enough gold dialog
        // If it is 0 (not visible), set it as unactive
        CanvasGroup canvasGroup = dialog.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            if (canvasGroup.alpha <= 0)
            {
                dialog.SetActive(false);
            }
        }
    }

    public void SetAINearest()
    {
        selectedStoredUnit.unitAI = Unit.AIType.Nearest;
    }

    public void SetAIRandom()
    {
        selectedStoredUnit.unitAI = Unit.AIType.Random;
    }

    public void SetAIDoT()
    {
        selectedStoredUnit.unitAI = Unit.AIType.Dot;
    }

    // Used to set the toggle of the currently active unit AI
    void SelectCurrentAIToggle()
    {       
        if (selectedStoredUnit.unitAI == Unit.AIType.Nearest)
        {
            aiPanel.SelectToggle(0);
            aiPanelExtended.SelectToggle(0);
        }
        else if (selectedStoredUnit.unitAI == Unit.AIType.Random)
        {
            aiPanel.SelectToggle(1);
            aiPanelExtended.SelectToggle(1);
        }
        else if (selectedStoredUnit.unitAI == Unit.AIType.Dot)
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
    */
}
