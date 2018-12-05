using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Randomizer))]
public class ProceduralSpawner : MonoBehaviour
{
    public static int WaveIndex;
    public static int EnemiesAlive;
    public static State CurrentState;

    public GameObject waveCompletePanel;
    public GameObject waveDefeatPanel;

    [Header("UI Components")]
    public Text enemiesAliveText;
    public Text countdownText;
    public Text bonusGoldText;
    public Button battleBtn;
    public PlayerStats player;

    [Header("Enemies")]
    public EnemyType[] enemyTypes;
    [Space]
    public Enemy[] bosses;

    [Space]

    [Header("Spawner Position")]
    public float yMinSpawnPos = -5f;
    public float yMaxSpawnPos = 0;

    public float yMinFlyerPos = 5f;
    public float yMaxFlyerPos = 0f;

    public float xSpawnPos = 10f;

    [Header("Testing")]
    public int startWave = 1;

    private Queue<Enemy> enemiesThisWave;
    private Coroutine spawnWave;
    private readonly float startCountdownTime = 5f;
    private float countdown;

    private Randomizer randomizer;

    public enum State
    {
        Waiting,
        Countdown,
        Spawning,
        WaveDone
    }

    void Start()
    {
        CurrentState = State.Waiting;
        randomizer = GetComponent<Randomizer>();
        WaveIndex = startWave;
    }

    void Update()
    {
        if (GameMaster.GameIsOver && CurrentState != State.WaveDone)
        {
            WaveDefeat();
            return;
        }

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

    void WaveDefeat()
    {
        StartCoroutine(DisplayWaveCompletePanel(waveDefeatPanel));
        StopCoroutine(spawnWave);
        ToggleBattleBtn();

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }

        EnemiesAlive = 0;
        PlayerStats.instance.health = player.startingHealth;
        GameMaster.GameIsOver = false;
    }

    void WaveComplete()
    {
        int endWaveGold = randomizer.GetEndWaveGold(WaveIndex);
        bonusGoldText.text = "Bonus: " + endWaveGold + "g";

        StartCoroutine(DisplayWaveCompletePanel(waveCompletePanel));
        ToggleBattleBtn();

        PlayerStats.instance.gold += endWaveGold;
        PlayerStats.instance.Gems++; // Gives the player a gem after each wave
        PlayerStats.instance.health = player.startingHealth;

        WaveIndex++;
    }

    public void BattleButtonClick()
    {
        if (Tutorial.instance.IsTutorial)
        {
            Tutorial.instance.EndTutorial();
        }

        countdownText.gameObject.SetActive(true);

        ToggleBattleBtn();
        countdown = startCountdownTime;

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
                spawnWave = StartCoroutine(SpawnWave());
                countdownText.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator SpawnWave()
    {
        CurrentState = State.Spawning;

        int enemyCount = randomizer.GetEnemyCount(WaveIndex);
        EnemiesAlive = enemyCount;

        float spawnInterval = randomizer.GetSpawnInterval(WaveIndex);

        CalculateEnemiesInWave();

        Debug.Log("Spawning wave: " + WaveIndex + " | Count: " + EnemiesAlive + " | Interval: " + spawnInterval);

        while (enemiesThisWave.Count > 0)
        {
            Enemy enemyToSpawn = enemiesThisWave.Dequeue();
            SpawnEnemy(enemyToSpawn);
            enemyCount--;

            yield return new WaitForSeconds(spawnInterval); // Consistent spawn interval
        }
    }

    void CalculateEnemiesInWave()
    {
        enemiesThisWave = new Queue<Enemy>();
        List<EnemyType> spawnableTypes = GetSpawnableTypes(WaveIndex);

        int totalEnemyGoldValue = 0;
        int goldAfterWaveComplete = 0;

        for (int i = 0; i < EnemiesAlive; i++)
        {
            int index = randomizer.GetWeightedIndex(spawnableTypes);

            Enemy enemyInWave = spawnableTypes[index].enemy;
            enemiesThisWave.Enqueue(enemyInWave);

            totalEnemyGoldValue += EnemyScaler.ScaleGold(enemyInWave.goldValue, WaveIndex);
            goldAfterWaveComplete = totalEnemyGoldValue + randomizer.GetEndWaveGold(WaveIndex);
        }

        if (randomizer.IsBossWave)
        {
            AddRandomBossToWave();
            Debug.Log("BOSS INCOMING!");
        }

        Debug.Log("Wave: " + WaveIndex + " | Enemy gold value: " + totalEnemyGoldValue);
        Debug.Log("Wave: " + WaveIndex + " | Wave complete value: " + goldAfterWaveComplete);

        foreach (Enemy enemy in enemiesThisWave)
        {
            //Debug.Log("Wave: " + WaveIndex + " | " + enemy);
        }
    }

    void AddRandomBossToWave()
    {
        int roll = Random.Range(0, bosses.Length - 1);

        EnemiesAlive++;
        enemiesThisWave.Enqueue(bosses[roll]);
    }

    public void EstimateTotalEarnings()
    {
        int totalGoldEarnedFromEnemies = 0;
        int grandTotal = 0;

        for (int i = 0; i < startWave; i++)
        {
            int enemyCount = randomizer.GetEnemyCount(startWave);

            for (int j = 0; j < enemyCount; j++)
            {
                List<EnemyType> spawnableTypes = GetSpawnableTypes(startWave);

                int index = randomizer.GetWeightedIndex(spawnableTypes);

                Enemy enemyInWave = spawnableTypes[index].enemy;

                totalGoldEarnedFromEnemies += EnemyScaler.ScaleGold(enemyInWave.goldValue, i);
            }

            grandTotal += totalGoldEarnedFromEnemies + randomizer.GetEndWaveGold(i);
        }

        Debug.Log("Estimated gold earned from enemies: " + totalGoldEarnedFromEnemies);
        Debug.Log("Estimated grand total earned: " + grandTotal);
    }

    void SpawnEnemy(Enemy enemyToSpawn)
    {
        Instantiate(enemyToSpawn, GetSpawnPosition(enemyToSpawn), Quaternion.identity);
    }

    Vector2 GetSpawnPosition(Enemy enemy)
    {
        float enemyHeight = enemy.GetComponent<SpriteRenderer>().bounds.size.y;

        Vector2 spawnPos = new Vector2();

        if (enemy.enemyType == Enemy.Type.Ground)
        {
            spawnPos = new Vector2(xSpawnPos, Random.Range(yMinSpawnPos + enemyHeight, yMaxSpawnPos - enemyHeight));
        }
        else if (enemy.enemyType == Enemy.Type.Flying)
        {
            spawnPos = new Vector2(xSpawnPos, Random.Range(yMinFlyerPos + enemyHeight, yMaxFlyerPos - enemyHeight));
        }

        return spawnPos;
    }

    List<EnemyType> GetSpawnableTypes(int waveIndex)
    {
        List<EnemyType> spawnableTypes = new List<EnemyType>();

        foreach (EnemyType enemyType in enemyTypes)
        {
            if (enemyType.waveStart <= waveIndex)
            {
                spawnableTypes.Add(enemyType);
            }
        }

        return spawnableTypes;
    }

    IEnumerator DisplayWaveCompletePanel(GameObject panel)
    {
        float timeTillFade = 2f;
        float timeTillDisable = .25f;

        CurrentState = State.WaveDone;

        panel.SetActive(true);
        yield return new WaitForSeconds(timeTillFade);

        Animator anim = panel.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("PanelExit");
        }

        yield return new WaitForSeconds(timeTillDisable);
        panel.SetActive(false);

        CurrentState = State.Waiting;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector3(xSpawnPos, yMinSpawnPos), new Vector3(xSpawnPos, yMaxSpawnPos));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(xSpawnPos, yMinFlyerPos), new Vector3(xSpawnPos, yMaxFlyerPos));
    }
}
