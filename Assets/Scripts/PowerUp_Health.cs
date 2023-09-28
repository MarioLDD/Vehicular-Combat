using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Health : MonoBehaviour
{
    [SerializeField] private int addhealth = 50;
    private HealthSystem healthSystem;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            healthSystem = other.GetComponentInParent<HealthSystem>();
            if((healthSystem.CurrentHealth + addhealth) < healthSystem.MaxHealth)
            {
                healthSystem.CurrentHealth += 50;
            }
            else
            {
                healthSystem.CurrentHealth += healthSystem.MaxHealth - healthSystem.CurrentHealth;
            }
            healthSystem.UpdateVisual();
            Destroy(gameObject);
        }
    }
}
