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
