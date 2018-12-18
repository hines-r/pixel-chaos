using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class DevPanelUI : MonoBehaviour
{
    public Toggle godModeToggle;
    public Toggle goldGemToggle;

    private Animator anim;
    private GameMaster gm;

    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameMaster.instance;

        godModeToggle.isOn = gm.isImmortal;
        goldGemToggle.isOn = gm.isBurdenedWithMoney;
    }

    public void ShowDevPanel()
    {
        gameObject.SetActive(true);
    }

    IEnumerator StartHideAnimation()
    {
        float delay = .1f;

        anim.SetTrigger("Hide");
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }

    public void HideDevPanel()
    {
        StartCoroutine(StartHideAnimation());
    }

    public void ToggleGodMode()
    {
        gm.isImmortal = godModeToggle.isOn;
    }

    public void ToggleInfiniteMoney()
    {
        gm.isBurdenedWithMoney = goldGemToggle.isOn;
    }
}
