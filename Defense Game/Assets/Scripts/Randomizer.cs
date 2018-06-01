using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public PlayerInfoUI playerInfoUI;

    private const float MaxTimePerWave = 30f; // Max of 30 seconds of spawn time per wave

    private float totalSpawnTime;

    public float GetSpawnInterval(float waveIndex)
    {
        waveIndex *= 0.15f;
        return 1f / (Mathf.Pow(waveIndex, 1.1f) + 5f + Mathf.Sin(waveIndex)) * 5f;
    }

    public int GetEnemyCount(float waveIndex)
    {
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
        return (int)Mathf.Pow(GetEnemyCount(waveIndex), 1.5f);
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
