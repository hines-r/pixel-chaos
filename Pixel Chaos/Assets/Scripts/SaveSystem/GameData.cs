using System.Collections;
using System.Collections.Generic;
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

    public GameData()
    {
        waveIndex = Spawner.WaveIndex;

        level = Player.instance.level;
        experience = Player.instance.experience;
        experienceToNextLevel = Player.instance.experienceToNextLevel;
        gold = Player.instance.gold;
        gems = Player.instance.gems;
    }
}
