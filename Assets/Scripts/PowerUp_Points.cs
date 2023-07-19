using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Points : MonoBehaviour
{
    [SerializeField] private int points = 1000;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            playerManager.WinScore(points);
            Destroy(gameObject);
        }
    }
}