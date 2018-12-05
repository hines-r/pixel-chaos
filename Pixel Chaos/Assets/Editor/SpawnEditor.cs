using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Spawn Wave"))
        {
            Spawner spawner = target as Spawner;
            spawner.StartCoroutine(spawner.SpawnWave());
        }

        if (GUILayout.Button("Estimate Earnings"))
        {
            Spawner spawner = target as Spawner;
            spawner.EstimateTotalEarnings();
        }
    }
}
