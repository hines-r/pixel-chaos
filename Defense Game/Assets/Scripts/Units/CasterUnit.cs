using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class CasterUnit : Unit, IPointerClickHandler
{
    [Header("Casters")]
    public GameObject castBar;
    public Image castProgress;

    private BoxCollider2D bc2d;

    protected bool isAbilityReady;

    protected override void Start()
    {
        base.Start();

        bc2d = GetComponent<BoxCollider2D>();
        bc2d.enabled = false;
    }

    protected override void Update()
    {
        if (GameMaster.GameIsOver)
        {
            return;
        }


        if (ProceduralSpawner.CurrentState == ProceduralSpawner.State.Spawning)
        {
            bc2d.enabled = true;
            castBar.SetActive(true);
        }
        else
        {
            bc2d.enabled = false;
            castBar.SetActive(false);
            ResetAttackTime();
            return;
        }

        if (nextAttackTime > attackSpeed)
        {
            isAbilityReady = true;
            return;
        }

        nextAttackTime += Time.deltaTime;
        castProgress.fillAmount = nextAttackTime / attackSpeed;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAbilityReady)
        {
            if (hasBurstAttack)
            {
                BurstAttack();
            }
            else
            {
                Attack();
            }

            ResetAttackTime();
            isAbilityReady = false;
        }
    }
}
