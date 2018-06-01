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

    private Randomizer randomizer;

    public enum State
    {
        Waiting,
        Countdown,
        Spawning,
        WaveComplete
    }

    void Start()
    {
        CurrentState = State.Waiting;
        randomizer = GetComponent<Randomizer>();
        WaveIndex = startWave;
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
        int endWaveGold = randomizer.GetEndWaveGold(WaveIndex);
        bonusGoldText.text = "Bonus: " + endWaveGold + "g";

        StartCoroutine(DisplayWaveCompletePanel());
        ToggleBattleBtn();

        PlayerStats.Gold += endWaveGold;
        PlayerStats.Gems++; // Gives the player a gem after each wave
        PlayerStats.Health = player.startingHealth;
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

    IEnumerator DisplayWaveCompletePanel()
    {
        float timeTillFade = 2f;
        float timeTillDisable = .25f;

        CurrentState = State.WaveComplete;

        waveCompletePanel.SetActive(true);
        yield return new WaitForSeconds(timeTillFade);

        Animator anim = waveCompletePanel.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("PanelExit");
        }

        yield return new WaitForSeconds(timeTillDisable);
        waveCompletePanel.SetActive(false);

        CurrentState = State.Waiting;
    }

    void SpawnEnemy()
    {
        List<EnemyType> spawnableTypes = GetSpawnableTypes();
        int index = randomizer.GetWeightedIndex(spawnableTypes);

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
