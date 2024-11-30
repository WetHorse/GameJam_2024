using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseView;
    [SerializeField] private GameObject _GameOverView;
    [SerializeField] private string scene = "Menu";
    private bool isPaused;
    private DefaultInputActions inputActions;


    private void Awake()
    {
        inputActions = new DefaultInputActions();
        inputActions.Enable();
        inputActions.Player.Pause.performed += OnPause;
        _pauseView.SetActive(false);
        _GameOverView.SetActive(false);
    }

    private void OnPause(InputAction.CallbackContext obj)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        _pauseView.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1f;
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

    private void OnDestroy()
    {
        inputActions.Player.Pause.performed -= OnPause;
    }
}
