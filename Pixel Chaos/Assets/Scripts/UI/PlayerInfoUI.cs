using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public Player player;
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

        levelBarImg.fillAmount = Player.instance.experience / player.experienceToNextLevel;
        levelText.text = Player.instance.level.ToString();

        healthBarImg.fillAmount = Player.instance.health / player.startingHealth ;
        healthText.text = Player.instance.health.ToString();

        goldText.text = Player.instance.gold.ToString();

        gemsText.text = Player.instance.Gems.ToString();
    }

    public void SetTime(float time)
    {
        currentTime = time;
    }
}
