using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _GameOverView;
    [SerializeField] private TMP_Text display, notificationDisplay; 
    [SerializeField] private string scene = "Menu";
    private bool isPaused;
    private DefaultInputActions inputActions;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Time.timeScale = 1.0f;
        Instance = this;
        inputActions = new DefaultInputActions();
        inputActions.Enable();
        inputActions.Player.Pause.performed += OnPause;
        _pauseView.SetActive(false);
        _GameOverView.SetActive(false);
    }

    public void SetNotification(string str)
    {
        notificationDisplay.text = str;
    }

    private void OnPause(InputAction.CallbackContext obj)
    {
        TogglePause();
    }

    public void OnDefeat ()
    {
        display.text = ScoreManager.Score;
        _GameOverView.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        _pauseView.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(scene);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    private void OnDestroy()
    {
        inputActions.Player.Pause.performed -= OnPause;
    }
}
