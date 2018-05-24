using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralSpawner : MonoBehaviour
{
    public static int WaveIndex;
    public static int EnemiesAlive;
    public static State CurrentState;

    [Header("UI Components")]
    public Text enemiesAliveText;
    public Text countdownText;
    public Text waveNumberText;
    public Button battleBtn;
    public PlayerStats player;

    [Header("Enemies")]
    public EnemyType[] enemyTypes;

    [Space]

    [Header("Spawner Position")]
    public float yMinSpawnPos = -5f;
    public float yMaxSpawnPos = 0;
    public float xSpawnPos = 10f;

    [Header("Testing")]
    public int startWave = 0;

    private float startCountdownTime = 5f;
    private float countdown;

    public enum State
    {
        Waiting,
        Countdown,
        Spawning
    }

    void Start()
    {
        CurrentState = State.Waiting;
        WaveIndex = startWave;
        waveNumberText.text = "Current Wave: " + (WaveIndex + 1);
    }

    void Update()
    {
        if (CurrentState == State.Waiting)
        {
            return;
        }

        if (CurrentState == State.Countdown)
        {
            countdownText.text = string.Format("{0:0}", countdown);
            return;
        }

        if (CurrentState == State.Spawning)
        {
            enemiesAliveText.text = "Enemies Alive: " + EnemiesAlive;

            if (EnemiesAlive > 0)
            {
                return;
            }
            else
            {
                WaveComplete();
            }
        }
    }

    void WaveComplete()
    {
        CurrentState = State.Waiting;

        ToggleBattleBtn();

        Debug.Log("End of wave gold: " + Randomizer.GetEndWaveGold(WaveIndex));

        PlayerStats.Gold += Randomizer.GetEndWaveGold(WaveIndex);
        PlayerStats.Gems++; // Gives the player a gem after each wave
        PlayerStats.Health = player.startingHealth;
    }

    public void BattleButtonClick()
    {
        waveNumberText.text = "Current Wave: " + (WaveIndex + 1);
        countdownText.gameObject.SetActive(true);

        ToggleBattleBtn();
        countdown = startCountdownTime;

        WaveIndex++;
        StartNextWave();
    }


    void ToggleBattleBtn()
    {
        // Sets the button on or off depending on its current state
        battleBtn.gameObject.SetActive(!battleBtn.gameObject.activeSelf);
    }

    public void StartNextWave()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        CurrentState = State.Countdown;

        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown--;

            if (countdown <= 0)
            {
                StartCoroutine(SpawnWave());
                countdownText.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator SpawnWave()
    {
        CurrentState = State.Spawning;

        int enemyCount = Randomizer.GetEnemyCount(WaveIndex);
        EnemiesAlive = enemyCount;

        float spawnInterval = Randomizer.GetSpawnInterval(WaveIndex);

        Debug.Log("Spawning wave: " + WaveIndex + " | Count: " + enemyCount + " | Interval: " + spawnInterval);

        while (enemyCount > 0)
        {
            SpawnEnemy();
            enemyCount--;
            yield return new WaitForSeconds(Random.Range(spawnInterval * .5f, spawnInterval * 2f));
        }
    }

    void SpawnEnemy()
    {
        List<EnemyType> spawnableTypes = GetSpawnableTypes();
        int index = Randomizer.GetWeightedIndex(spawnableTypes);

        GameObject enemyToSpawn = spawnableTypes[index].enemy;

        Instantiate(enemyToSpawn, GetSpawnPosition(enemyToSpawn), Quaternion.identity);
    }

    Vector2 GetSpawnPosition(GameObject enemy)
    {
        float enemyHeight = enemy.GetComponent<SpriteRenderer>().bounds.size.y;

        return new Vector2(xSpawnPos, Random.Range(yMinSpawnPos + enemyHeight, yMaxSpawnPos - enemyHeight));
    }

    List<EnemyType> GetSpawnableTypes()
    {
        List<EnemyType> spawnableTypes = new List<EnemyType>();

        foreach (EnemyType enemyType in enemyTypes)
        {
            if (enemyType.waveStart <= WaveIndex)
            {
                spawnableTypes.Add(enemyType);
            }
        }

        return spawnableTypes;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector3(xSpawnPos, yMinSpawnPos), new Vector3(xSpawnPos, yMaxSpawnPos));
    }
}
