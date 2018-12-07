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

        // Searches for any units saved within the game data file
        // If any are found, gets the UnitButton value from the button dictionary
        // and places the corresponding unit on the node and unlocks the button
        for (int i = 0; i < data.nodeUnitNames.Count; i++)
        {
            if (data.nodeUnitNames[i] != null)
            {
                Unit unitToLoad = unitButtons[data.nodeUnitNames[i]].unit;

                if (unitToLoad != null)
                {
                    UnitButton buttonToUpdate = unitButtons[data.nodeUnitNames[i]];
                    buttonToUpdate.UnlockButton();

                    // TODO: set appropriate upgrades for the loaded unit

                    UnitManager.instance.nodes[i].PlaceUnit(unitToLoad);
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
