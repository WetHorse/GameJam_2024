using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public static PlayerHealthSystem Instance;
    [SerializeField] private Image display;
    [SerializeField,Range(0,100)] private float currentHealth, maxHealth;
    [SerializeField] private bool godMod;
    public bool GodMod { get => godMod; set { godMod = value; } }
    [SerializeField] private UnityEvent onDeath, onDamageTaken, onHealApplied;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public void Damage (float damage)
    {
        if (godMod) return;
        DamageIgnoreGodMod(damage);
    }


    public void DamageIgnoreGodMod(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Player died");
            UpdateDisplay();
            PauseManager.Instance.OnDefeat();
            return;
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
}
