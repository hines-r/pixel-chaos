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
    public List<string> unlockedUnits = new List<string>();
    public List<int> unitLevels = new List<int>();
    public List<bool> unitActiveStatuses = new List<bool>();

    public List<string> nodeUnitNames = new List<string>();

    public GameData()
    {
        waveIndex = Spawner.WaveIndex;

        level = Player.instance.level;
        experience = Player.instance.experience;
        experienceToNextLevel = Player.instance.experienceToNextLevel;
        gold = Player.instance.gold;
        gems = Player.instance.gems;

        foreach (Transform childUnit in UnitManager.instance.transform)
        {
            Unit unit = childUnit.GetComponent<Unit>();

            if (unit != null)
            {
                unlockedUnits.Add(unit.unitName);
                unitLevels.Add(unit.level);
                unitActiveStatuses.Add(unit.gameObject.activeSelf);
            }
        }

        Node[] nodes = UnitManager.instance.nodes;

        foreach (Node node in nodes)
        {
            if (node.IsUnitOnNode())
            {
                nodeUnitNames.Add(node.GetUnitOnNode().unitName);
            }
            else
            {
                nodeUnitNames.Add(null);
            }
        }
    }
}
