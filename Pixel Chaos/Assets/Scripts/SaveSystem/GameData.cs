using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Spawner data
    public int waveIndex;

    // Player data
    public int level;
    public float experience;
    public float experienceToNextLevel;
    public int gold;
    public int gems;

    // Scene/Unit data
    public List<string> nodeUnitNames = new List<string>();

    public GameData()
    {
        waveIndex = Spawner.WaveIndex;

        level = Player.instance.level;
        experience = Player.instance.experience;
        experienceToNextLevel = Player.instance.experienceToNextLevel;
        gold = Player.instance.gold;
        gems = Player.instance.gems;

        Node[] nodes = UnitManager.instance.nodes;

        for (int i = 0; i < nodes.Length; i++)
        {
            Node tempNode = nodes[i];

            if (tempNode.IsUnitOnNode())
            {
                nodeUnitNames.Add(tempNode.GetUnitOnNode().unitName);
            }
            else
            {
                nodeUnitNames.Add(null);
            }
        }

        // TODO: get save data for each unlocked unit ex. level, upgrades, etc
    }
}
