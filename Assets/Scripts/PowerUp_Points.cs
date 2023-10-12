using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Points : MonoBehaviour
{
    [SerializeField] private int points = 1000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            PlayerManager playerManager = other.GetComponentInParent<PlayerManager>();
            playerManager.WinScore(points);
            Destroy(gameObject);
        }
    }
}