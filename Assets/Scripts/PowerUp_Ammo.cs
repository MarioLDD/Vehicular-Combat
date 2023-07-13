using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Ammo: MonoBehaviour
{
    [SerializeField] private int ammunition = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<Weapon>().Addmunitions(ammunition);
            Destroy(gameObject);
        }
    }
}