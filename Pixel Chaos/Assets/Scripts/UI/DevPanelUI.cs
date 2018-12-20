using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Animator))]
public class DevPanelUI : MonoBehaviour
{
    public Toggle godModeToggle;
    public Toggle goldGemToggle;

    public InputField waveInput;
    public InputField goldInput;
    public InputField gemInput;

    private Animator anim;
    private GameMaster gm;

    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameMaster.instance;

        godModeToggle.isOn = gm.isImmortal;
        goldGemToggle.isOn = gm.isBurdenedWithMoney;
    }

    void OnEnable()
    {
        waveInput.text = Spawner.WaveIndex.ToString();
        goldInput.text = Player.instance.gold.ToString();
        gemInput.text = Player.instance.gems.ToString();
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

    public void SetWaveNumber()
    {
        int number;
        if (Int32.TryParse(waveInput.text, out number))
        {
            Spawner.WaveIndex = number;
        }
    }

    public void SetGold()
    {
        int amount;
        if (Int32.TryParse(goldInput.text, out amount))
        {
            Player.instance.gold = amount;
        }
    }

    public void SetGems()
    {
        int amount;
        if (Int32.TryParse(gemInput.text, out amount))
        {
            Player.instance.gems = amount;
        }
    }
}
