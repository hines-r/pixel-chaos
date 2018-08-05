using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject ui;
    public Button retryBtn;

    void Start()
    {
        Toggle();
    }

    void Toggle()
    {
        ui.SetActive(true);
        Time.timeScale = .5f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
