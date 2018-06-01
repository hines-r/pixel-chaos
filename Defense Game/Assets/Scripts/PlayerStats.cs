using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Info")]
    public static int Level;
    public int startingLevel = 1;

    public static float Experience;
    public float startingExperiene;
    public float experienceToNextLevel = 150f;

    public static float Health;
    public float startingHealth = 100f;

    public static int Gold;
    public int startGold = 100;

    public static int Gems;
    public int startingGems = 0;

    public static int Rounds;

    private readonly float multiplier = 1.18f;

    void Start()
    {
        Health = startingHealth;
        Experience = startingExperiene;
        Level = startingLevel;
        Gold = startGold;
        Gems = startingGems;

        Rounds = 0;
    }

    void Update()
    {
        if (Experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        float carryOverXp = Experience - experienceToNextLevel;

        experienceToNextLevel *= multiplier;
        Experience = carryOverXp;

        Level++;
    }
}
