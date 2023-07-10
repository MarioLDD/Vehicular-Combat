using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IHealthSystem
{
    private int maxHealth;
    public UnityEvent onHealthZero;
    private int currentHealth;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } } 

    public int CurrentHealth { get { return currentHealth; } }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
           onHealthZero.Invoke();
        }        
    }
}
