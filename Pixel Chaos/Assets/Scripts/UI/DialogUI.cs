using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class DialogUI : MonoBehaviour
{
    public GameObject dialog;
    public Text dialogText;

    private Animator anim;

    private readonly float timeToWait = 5f; // Time till destroy

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    IEnumerator ActivateDialog()
    {
        yield return new WaitForSeconds(timeToWait);

        // After waiting a bit, check the alpha of the not enough gold dialog
        // If it is 0 (not visible), set it as unactive
        CanvasGroup canvasGroup = dialog.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            if (canvasGroup.alpha <= 0)
            {
                dialog.SetActive(false);
            }
        }
    }

    public void DisplayDialog(string textToDisplay)
    {
        dialogText.text = textToDisplay;
        dialog.SetActive(true);
        anim.Play("DialogEntry", -1, 0);
        StartCoroutine(ActivateDialog());
    }
}
