using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    #region Singleton
    public static UnitManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one UnitManager in scene!");
            return;
        }

        instance = this;
    }
    #endregion

    public List<GameObject> unlockedUnits;
    public Node[] nodes;

    void Start()
    {
        unlockedUnits = new List<GameObject>();
        nodes = FindObjectsOfType<Node>();
    }

    public void UnlockUnit(GameObject unitToUnlock)
    {
        if (!unlockedUnits.Contains(unitToUnlock))
        {
            unlockedUnits.Add(unitToUnlock);
        }
    }
}
