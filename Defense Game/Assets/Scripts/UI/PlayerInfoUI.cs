using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStats))]
public class PlayerInfoUI : MonoBehaviour
{
    public Image levelBarImg;
    public Text levelText;

    public Image healthBarImg;
    public Text healthText;

    public Text goldText;
    public Text gemsText;

    private PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

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
