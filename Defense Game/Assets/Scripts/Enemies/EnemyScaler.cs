using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScaler
{
    public static float ScaleHealth(float baseHealth, int waveIndex)
    {
        int x = (int)(waveIndex / .9f);
        return baseHealth + Mathf.Pow(waveIndex, 1.75f) + Mathf.Sin(Mathf.Pow(waveIndex, 1.3f));
    }

    public static int ScaleExpValue(int baseExp, int waveIndex)
    {
        return baseExp + (int)(Mathf.Pow(waveIndex, 1.4f) + Mathf.Sin(Mathf.Pow(waveIndex, 1.35f)));
    }

    public static int ScaleGold(int baseGold, int waveIndex)
    {
        return baseGold + (int)((Mathf.Pow(waveIndex, 1.1f) + Mathf.Sin(waveIndex)) / 10f);
    }
}
