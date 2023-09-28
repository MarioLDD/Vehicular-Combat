using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Shield : MonoBehaviour
{
    [SerializeField] private int shieldHealth;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponentInParent<ShieldSystem>().PrepareShield(shieldHealth);
            Destroy(gameObject);
        }
    }
}