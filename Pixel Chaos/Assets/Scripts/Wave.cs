using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    public int[] count;
    public float[] waitToSpawn;
    public float[] rate;
}
