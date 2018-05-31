﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public PlayerStats player;

    public Image levelBarImg;
    public Text levelText;

    public Image healthBarImg;
    public Text healthText;

    public Text goldText;
    public Text gemsText;

    void Update()
    {
        levelBarImg.fillAmount = PlayerStats.Experience / player.experienceToNextLevel;
        levelText.text = PlayerStats.Level.ToString();

        healthBarImg.fillAmount = PlayerStats.Health / player.startingHealth ;
        healthText.text = PlayerStats.Health.ToString();

        goldText.text = "Gold: " + PlayerStats.Gold;

        gemsText.text = "Gems: " + PlayerStats.Gems;
    }
}
