using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static string Score;
    [SerializeField] private TMP_Text display;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Score = "0km";
        Instance = this;
    }

    private void Update()
    {
        if (PlayerHealthSystem.Instance == null) return;
        Score = $"{Mathf.CeilToInt(PlayerHealthSystem.Instance.transform.position.y)}km";
        display.text = Score;
    }
}
