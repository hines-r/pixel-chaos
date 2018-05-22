using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer
{
    public static float GetSpawnInterval(float waveIndex)
    {
        waveIndex /= .8f;
        return 1f / (Mathf.Pow(waveIndex, 1.2f) + 2f + Mathf.Sin(waveIndex)) * 8f;
    }

    public static int GetEnemyCount(float waveIndex)
    {
        waveIndex /= 0.6f;
        return (int)(Mathf.Pow(waveIndex, 1.4f) + 5f + Mathf.Sin(waveIndex));
    }

    public static int GetEndWaveGold(int waveIndex)
    {
        return (int)Mathf.Pow(GetEnemyCount(waveIndex), 1.5f);
    }

    public static int GetWeightedIndex(List<EnemyType> types)
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
}
