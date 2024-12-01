using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private GameObject winScreen;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public void GameWon ()
    {
        scoreDisplay.text = ScoreManager.Score;
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Yeaaah win!");
    }
}
