using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Unit), typeof(BoxCollider2D))]
public class Caster : MonoBehaviour, IPointerClickHandler
{
    [Header("Caster")]
    public GameObject castArea;
    public Image castProgress;

    private bool isAbilityReady;

    private Unit unit;
    private BoxCollider2D bc2d;

    void Start()
    {
        unit = GetComponent<Unit>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (ProceduralSpawner.CurrentState == ProceduralSpawner.State.Spawning)
        {
            bc2d.enabled = true;
            castArea.SetActive(true);
        }
        else
        {
            bc2d.enabled = false;
            castArea.SetActive(false);
            unit.ResetAttackTime();
            return;
        }

        if (unit.GetNextAttackTime() > unit.attackSpeed)
        {
            isAbilityReady = true;
            return;
        }

        castProgress.fillAmount = unit.GetNextAttackTime() / unit.attackSpeed;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAbilityReady && !unit.isPassiveCaster)
        {
            if (unit.hasBurstAttack)
            {
                unit.BurstAttack();
            }
            else
            {
                unit.Attack();
            }

            unit.ResetAttackTime();
            isAbilityReady = false;
        }
    }

}
