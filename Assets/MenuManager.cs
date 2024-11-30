using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainView;
    [SerializeField] private GameObject _creditView;
    [SerializeField] private GameObject _controlsView;
    [SerializeField] private string _scene = "Main";

    private void Awake()
    {
        _mainView.SetActive(true);
        _creditView.SetActive(false);
        _controlsView.SetActive(false);
    }

    public void GoToCredits()
    {
        _mainView.SetActive(false);
        _creditView.SetActive(true);
    }

    public void GoToControls()
    {
        _mainView.SetActive(false);
        _controlsView.SetActive(true);
    }

    public void GoBackFromControls()
    {
        _controlsView.SetActive(false);
        _mainView.SetActive(true);

    }

    public void GoBackFromCredits()
    {
        _creditView.SetActive(false);
        _mainView.SetActive(true);

    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(_scene);
    }
}
