using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public UnityEvent onHealthZero;
    private int currentHealth;
    private IUpdateHealth updateHealth;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } } 

    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    private void Start()
    {
        currentHealth = maxHealth;
        updateHealth = GetComponentInChildren<IUpdateHealth>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateVisual();
        if (currentHealth < 0)
        {
           onHealthZero.Invoke();
        }        
    }

    public void UpdateVisual()
    {
        updateHealth.UpdateHealth(maxHealth, currentHealth);
    }
}
