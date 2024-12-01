using System.Collections;
using UnityEngine;

public class Border : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] private float looseTime = 5f;
    private float currentLooseTime;

    private void Start()
    {
        currentLooseTime = looseTime;
    }

    private void Update()
    {
        if (playerInRange)
        {
            currentLooseTime -= Time.deltaTime;
            PauseManager.Instance.SetNotification($"{Mathf.Ceil(currentLooseTime)}");

            if (currentLooseTime <= 0)
            {
                LoseGame();
            }
        }
        else if (currentLooseTime < looseTime)
        {
            currentLooseTime = looseTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            PauseManager.Instance.SetNotification("");
        }
    }

    private void LoseGame()
    {
        PauseManager.Instance.SetNotification("");
        PauseManager.Instance.OnDefeat();
    }
}
