using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [Header("Unit Prefab")]
    public AttackingUnit unit;

    [Header("Image")]
    public Image unitImg;

    [Header("Name")]
    public Text unitName;

    [Header("Button Settings")]
    public Color lockedColor;
    public Color unlockedColor;

    void Start()
    {
        unitImg.sprite = unit.unitSprite;
        unitName.text = unit.unitName;
    }

}
