using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public PlayerStats player;
    public Randomizer randomizer;

    public Image waveBarImg;
    public Text waveBarText;

    public Image levelBarImg;
    public Text levelText;

    public Image healthBarImg;
    public Text healthText;

    public Text goldText;
    public Text gemsText;

    private float currentTime;
    private readonly float fillSpeed = 4f;

    void Update()
    {
        if (ProceduralSpawner.CurrentState == ProceduralSpawner.State.Spawning)
        {
            currentTime -= Time.deltaTime;
            waveBarImg.fillAmount = currentTime / randomizer.GetTotalSpawnTime();
            waveBarText.text = "Wave " + ProceduralSpawner.WaveIndex;
        }
        else
        {
            waveBarImg.fillAmount = Mathf.Lerp(waveBarImg.fillAmount, 1, fillSpeed * Time.deltaTime);
        }

        levelBarImg.fillAmount = PlayerStats.Experience / player.experienceToNextLevel;
        levelText.text = PlayerStats.Level.ToString();

        healthBarImg.fillAmount = PlayerStats.Health / player.startingHealth ;
        healthText.text = PlayerStats.Health.ToString();

        goldText.text = "Gold: " + PlayerStats.Gold;

        gemsText.text = "Gems: " + PlayerStats.Gems;
    }

    public void SetTime(float time)
    {
        currentTime = time;
    }
}
