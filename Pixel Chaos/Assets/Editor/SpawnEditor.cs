using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralSpawner))]
public class SpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Spawn Wave"))
        {
            ProceduralSpawner spawner = target as ProceduralSpawner;
            spawner.StartCoroutine(spawner.SpawnWave());
        }

        if (GUILayout.Button("Estimate Earnings"))
        {
            ProceduralSpawner spawner = target as ProceduralSpawner;
            spawner.EstimateTotalEarnings();
        }
    }
}
