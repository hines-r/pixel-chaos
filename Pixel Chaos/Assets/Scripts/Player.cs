using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Info")]
    public int level;
    public int startingLevel = 1;

    public float experience;
    public float startingExperiene;
    public float experienceToNextLevel = 150f;

    public float health;
    public float startingHealth = 100f;

    public int gold;
    public int startGold = 100;

    public int Gems;
    public int startingGems = 0;

    public int rounds;

    private readonly float multiplier = 1.18f;

    #region Singleton

    public static Player instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Player in scene!");
            return;
        }

        instance = this;
    }

    #endregion

    void Start()
    {
        health = startingHealth;
        experience = startingExperiene;
        level = startingLevel;
        gold = startGold;
        Gems = startingGems;

        rounds = 0;
    }

    void Update()
    {
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        float carryOverXp = experience - experienceToNextLevel;

        experienceToNextLevel *= multiplier;
        experience = carryOverXp;

        level++;
    }
}
