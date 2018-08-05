using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public Enemy enemy;

    // Which wave to start and stop spawning
    public int waveStart;

    // Chances of spawning within wave
    public float weight;
}
