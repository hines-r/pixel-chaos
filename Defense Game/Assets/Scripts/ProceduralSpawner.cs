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

    [Header("Spawner Position")]
    public float yMinSpawnPos = -5f;
    public float yMaxSpawnPos = 0;
    public float xSpawnPos = 10f;

    [Header("Testing")]
    public int startWave = 0;

    private readonly float startCountdownTime = 5f;
    private float countdown;
    private List<GameObject> activeEnemies;

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
        activeEnemies = new List<GameObject>();
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
        ToggleBattleBtn();

        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }

        PlayerStats.Health = player.startingHealth;
        GameMaster.GameIsOver = false;
    }

    void WaveComplete()
    {
        int endWaveGold = randomizer.GetEndWaveGold(WaveIndex);
        bonusGoldText.text = "Bonus: " + endWaveGold + "g";

        StartCoroutine(DisplayWaveCompletePanel(waveCompletePanel));
        ToggleBattleBtn();

        PlayerStats.Gold += endWaveGold;
        PlayerStats.Gems++; // Gives the player a gem after each wave
        PlayerStats.Health = player.startingHealth;

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
                StartCoroutine(SpawnWave());
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

        Debug.Log("Spawning wave: " + WaveIndex + " | Count: " + enemyCount + " | Interval: " + spawnInterval);

        while (enemyCount > 0)
        {
            SpawnEnemy();
            enemyCount--;

            yield return new WaitForSeconds(spawnInterval); // Consistent spawn interval
        }
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

    void SpawnEnemy()
    {
        List<EnemyType> spawnableTypes = GetSpawnableTypes();
        int index = randomizer.GetWeightedIndex(spawnableTypes);

        GameObject enemyToSpawn = spawnableTypes[index].enemy;

        GameObject spawnedEnemy = Instantiate(enemyToSpawn, GetSpawnPosition(enemyToSpawn), Quaternion.identity);
        activeEnemies.Add(spawnedEnemy);
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
