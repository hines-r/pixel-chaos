using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public PlayerInfoUI playerInfoUI;

    private const float MaxTimePerWave = 30f; // Max of 30 seconds of spawn time per wave
    private float totalSpawnTime;

    public bool IsBossWave { get; set; }

    public float GetSpawnInterval(float waveIndex)
    {
        waveIndex *= 0.25f;
        return 1f / (Mathf.Pow(waveIndex, 1.1f) + 9f + Mathf.Sin(waveIndex)) * 10f;
    }

    public int GetEnemyCount(float waveIndex)
    {
        if (waveIndex % 10 == 0)
        {
            IsBossWave = true;
            return 1;
        }
        else
        {
            IsBossWave = false;
        }

        float count = (Mathf.Pow(waveIndex, 1.4f) + 5f + Mathf.Sin(waveIndex));

        totalSpawnTime = count * GetSpawnInterval(waveIndex);

        // Adjusts the number of enemies so the time of spawning never exceeds the max time
        if (totalSpawnTime > MaxTimePerWave)
        {
            count = MaxTimePerWave / GetSpawnInterval(waveIndex);
            totalSpawnTime = count * GetSpawnInterval(waveIndex);
        }
    
        playerInfoUI.SetTime(totalSpawnTime);

        return (int)count;
    }

    public int GetEndWaveGold(int waveIndex)
    {
        return (int)Mathf.Pow(waveIndex, 1.9f) + 75;
    }

    public int GetWeightedIndex(List<EnemyType> types)
    {
        float totalSpawnWeight = 0f;

        foreach (EnemyType enemyType in types)
        {
            totalSpawnWeight += enemyType.weight;
        }

        float pick = Random.value * totalSpawnWeight;
        float cumulataiveWeight = types[0].weight;

        int index = 0;

        while (pick > cumulataiveWeight && index < types.Count - 1)
        {
            index++;
            cumulataiveWeight += types[index].weight;
        }

        return index;
    }

    public float GetTotalSpawnTime()
    {
        return totalSpawnTime;
    }
}
