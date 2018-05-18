using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static int EnemiesAlive;
    public static bool IsSpawning;

    public PlayerStats player;

    public Text enemiesAliveText;
    public Text nextWaveText;
    public Text waveNumberText;
    public Button battleBtn;

    public float timeBetweenWaves = 3f;
    private float startCountdownTime = 5f;
    private float countdown;

    public Wave[] waves;

    private float nextSpawnTime;
    private Vector2 screenHalfSizeWorldUnits;

    private int waveIndex = 0;
    private int previousWaveIndex;

    public enum State
    {
        Waiting,
        Countdown,
        Spawning
    }

    private State currentState;

    // Use this for initialization
    void Start()
    {
        IsSpawning = false;
        EnemiesAlive = 0;
        countdown = startCountdownTime;
        waveNumberText.text = "Current Wave: " + (waveIndex + 1);
        screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSpawning)
        {
            return;
        }

        enemiesAliveText.text = "Enemies Remaining: " + EnemiesAlive;

        // Checks if the current wave has been killed off
        if (EnemiesAlive > 0)
        {
            return;
        }

        // Checks if current waveIndex has incremented so the game can wait for the player
        if (waveIndex > previousWaveIndex)
        {
            WaveComplete();

            if (waveIndex == waves.Length)
            {
                GameMaster.GameIsWon = true;
            }

            return;
        }

        if (countdown <= 0f)
        {
            nextWaveText.gameObject.SetActive(false);
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            waveNumberText.text = "Current Wave: " + (waveIndex + 1);
            return;
        }

        nextWaveText.text = string.Format("{0:0}", countdown);
    }

    IEnumerator SpawnWave()
    {
        currentState = State.Spawning;
        IsSpawning = true;

        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.enemies.Length; i++)
        {
            EnemiesAlive += wave.count[i];
        }

        for (int i = 0; i < wave.enemies.Length; i++)
        {
            yield return new WaitForSeconds(wave.waitToSpawn[i]);
            StartCoroutine(SpawnEnemyType(i));
        }

        previousWaveIndex = waveIndex;
        waveIndex++;
    }

    IEnumerator SpawnEnemyType(int enemyIndex)
    {
        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.count[enemyIndex]; i++)
        {
            SpawnEnemy(wave.enemies[enemyIndex]);
            yield return new WaitForSeconds(1f / wave.rate[enemyIndex]);
        }
    }

    IEnumerator StartCountdown()
    {
        currentState = State.Countdown;

        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown--;
        }
    }

    void WaveComplete()
    {
        currentState = State.Waiting;
        IsSpawning = false;

        ToggleBattleBtn();

        PlayerStats.Gems++; // Gives the player a gem after each wave
        PlayerStats.Health = player.startingHealth;
    }

    void SpawnEnemy(GameObject enemy)
    {
        // Spawns an enemy randomly within the bounds of the screen
        float enemyWidth = enemy.GetComponent<SpriteRenderer>().bounds.size.x;
        float enemyHeight = enemy.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector2 spawnPosition = new Vector2(screenHalfSizeWorldUnits.x + enemyWidth, Random.Range(-screenHalfSizeWorldUnits.y + enemyHeight, -enemyHeight));

        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }

    public void BattleButtonClick()
    {
        if (waveIndex < waves.Length)
        {
            nextWaveText.gameObject.SetActive(true);
            ToggleBattleBtn();
            StartCoroutine(StartCountdown());
            IsSpawning = true;
            previousWaveIndex = waveIndex;
            countdown = startCountdownTime;
        }
    }

    void ToggleBattleBtn()
    {
        // Sets the button on or off depending on its current state
        battleBtn.gameObject.SetActive(!battleBtn.gameObject.activeSelf);
    }
}
