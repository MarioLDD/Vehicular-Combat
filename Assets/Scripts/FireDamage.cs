using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [SerializeField] private int fireDamage = 2;
    private GameObject currentPlayer;

    void Start()
    {
        if (currentPlayer == null)
        {
            currentPlayer = gameObject.transform.parent.gameObject;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponentInParents(out HealthSystem healthSystem))
        {
            if (healthSystem.gameObject != currentPlayer)
            {
                healthSystem.TakeDamage(fireDamage, currentPlayer);
            }
        }
    }
}