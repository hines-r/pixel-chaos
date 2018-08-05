using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Tutorial : MonoBehaviour
{
    #region Singleton
    public static Tutorial instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one Tutorial in scene!");
        }

        instance = this;
    }
    #endregion

    public GameObject phase1;
    public GameObject phase2;
    public GameObject phase3;
    public GameObject phase4;
    public GameObject phase5;

    public bool IsTutorial = true;

    private Animator anim;

    void Start()
    {
        if (IsTutorial)
        {
            phase1.SetActive(true);
        }

        anim = GetComponent<Animator>();
    }

    public void TriggerPhaseTwo()
    {
        phase1.SetActive(false);
        phase2.SetActive(true);
        anim.SetTrigger("Phase2");
    }

    public void TriggerPhaseThree()
    {
        if (phase3.activeSelf)
        {
            phase3.SetActive(false);
            phase2.SetActive(true);
            anim.SetTrigger("Phase2");
            return;
        }

        phase2.SetActive(false);
        phase3.SetActive(true);
    }

    public void TriggerPhaseFour()
    {
        phase2.SetActive(false);
        phase4.SetActive(true);
        phase3.SetActive(false);
        anim.SetTrigger("Phase4");
    }

    public void TriggerPhaseFive()
    {
        phase5.SetActive(true);
        phase4.SetActive(false);
        anim.SetTrigger("Phase5");
    }

    public void EndTutorial()
    {
        IsTutorial = false;

        phase1.SetActive(false);
        phase2.SetActive(false);
        phase3.SetActive(false);
        phase4.SetActive(false);
        phase5.SetActive(false);
    }
}
