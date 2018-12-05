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
        if (Spawner.CurrentState == Spawner.State.Spawning)
        {
            currentTime -= Time.deltaTime;
            waveBarImg.fillAmount = currentTime / randomizer.GetTotalSpawnTime();
        }
        else
        {
            waveBarText.text = "Wave " + Spawner.WaveIndex;
            waveBarImg.fillAmount = Mathf.Lerp(waveBarImg.fillAmount, 1, fillSpeed * Time.deltaTime);
        }

        levelBarImg.fillAmount = PlayerStats.instance.experience / player.experienceToNextLevel;
        levelText.text = PlayerStats.instance.level.ToString();

        healthBarImg.fillAmount = PlayerStats.instance.health / player.startingHealth ;
        healthText.text = PlayerStats.instance.health.ToString();

        goldText.text = PlayerStats.instance.gold.ToString();

        gemsText.text = PlayerStats.instance.Gems.ToString();
    }

    public void SetTime(float time)
    {
        currentTime = time;
    }
}
