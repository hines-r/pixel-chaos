using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScaler
{
    public static float ScaleHealth(float baseHealth, int waveIndex)
    {
        return baseHealth + Mathf.Pow(waveIndex, 1.5f);
    }

    public static int ScaleExpValue(int baseExp, int waveIndex)
    {
        return baseExp + (int)Mathf.Pow(waveIndex, 1.3f);
    }

    public static int ScaleGold(int baseGold, int waveIndex)
    {
        return baseGold + (int)Mathf.Pow(waveIndex, 1.001f) / 4;
    }
}
