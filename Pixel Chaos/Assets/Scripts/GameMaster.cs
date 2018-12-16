using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    #region Singleton
    public static GameMaster instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one GameMaster in scene!");
            return;
        }

        instance = this;
    }
    #endregion

    public static bool GameIsOver = false;
    public static bool GameIsWon = false;

    public GameObject gameOverUI;
    public GameObject gameWinUI;
    public UnitSelectionUI unitSelectionUI;

    [Header("Dev Hacks")]
    public bool isImmortal;
    public bool isBurdenedWithMoney;

    void Start()
    {
        GameIsWon = false;
        GameIsOver = false;
    }

    void Update()
    {
        if (GameIsOver)
        {
            return;
        }

        if (GameIsWon)
        {
            WinGame();
        }

        if (Player.instance.health <= 0 && !isImmortal)
        {
            EndGame();
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        Spawner.WaveIndex = data.waveIndex;

        Player.instance.level = data.level;
        Player.instance.experience = data.experience;
        Player.instance.experienceToNextLevel = data.experienceToNextLevel;
        Player.instance.gold = data.gold;
        Player.instance.gems = data.gems;

        LoadUnits(data);
    }

    void LoadUnits(GameData data)
    {
        Dictionary<string, UnitButton> unitButtons = unitSelectionUI.buttons;

        for (int i = 0; i < data.unlockedUnits.Count; i++)
        {
            string unitName = data.unlockedUnits[i];

            Unit unitToLoad = null;

            if (!UnitManager.instance.unlockedUnits.ContainsKey(unitName))
            {
                // Gets the unit that needs to be loaded in and sets appropriate level and active status
                unitToLoad = Instantiate(unitButtons[unitName].unit);
                unitToLoad.SetLevel(data.unitLevels[i]);
                unitToLoad.gameObject.SetActive(data.unitActiveStatuses[i]);
                unitToLoad.transform.parent = UnitManager.instance.transform;
                UnitManager.instance.UnlockUnit(unitToLoad);

                // Updates the button within unit selection panel with original unit status
                UnitButton buttonToUpdate = unitButtons[unitName];
                buttonToUpdate.UnlockButton();
                buttonToUpdate.UpdateButton(unitToLoad);

                if (data.nodeUnitNames.Contains(unitName))
                {
                    int nodeIndex = data.nodeUnitNames.IndexOf(unitName);
                    Node nodeWithUnit = UnitManager.instance.nodes[nodeIndex];
                    nodeWithUnit.PlaceUnit(unitToLoad);
                }
            }
        }
    }

    void EndGame()
    {
        GameIsOver = true;
    }

    void WinGame()
    {
        gameWinUI.SetActive(true);
    }
}
