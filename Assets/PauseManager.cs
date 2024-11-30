using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _GameOverView;
    [SerializeField] private string scene = "Menu";
    private bool isPaused;

    private void Awake()
    {
        _pauseView.SetActive(false);
        _GameOverView.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsPaused())
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) Resume();
    }

    public void Pause()
    {
        _pauseView.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void Resume()
    {
        _pauseView.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(scene);
        Time.timeScale = 1.0f;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
