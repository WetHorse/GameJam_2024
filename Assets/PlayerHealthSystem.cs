using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private Image display;
    [SerializeField,Range(0,100)] private float currentHealth, maxHealth;
    [SerializeField] private bool godMod;

    [SerializeField] private UnityEvent onDeath, onDamageTaken, onHealApplied;

    public void Damage (float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage,0, maxHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
            onDeath?.Invoke();
        }
        else
        {
            Debug.Log("Player took damage");
            onDamageTaken?.Invoke();
        }
        UpdateDisplay();
    }

    public void Heal (float healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        onHealApplied?.Invoke();
        UpdateDisplay();
    }

    public void UpdateDisplay ()
    {
        display.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }

    private void Update()
    {
        UpdateDisplay();
    }
}
