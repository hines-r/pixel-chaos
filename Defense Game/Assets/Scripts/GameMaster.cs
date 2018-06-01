﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static bool GameIsOver = false;
    public static bool GameIsWon = false;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public bool isImmortal; // Dev hacks

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

        if (PlayerStats.Health <= 0 && !isImmortal)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        GameIsOver = true;
        gameOverUI.SetActive(true);
    }

    void WinGame()
    {
        gameWinUI.SetActive(true);
    }
}
