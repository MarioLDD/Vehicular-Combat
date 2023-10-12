using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Flames : MonoBehaviour
{
    [SerializeField] private int particleLiveTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            other.GetComponentInParent<FlamesSystem>().PreparePowerUp(particleLiveTime);
            Destroy(gameObject);
        }
    }
}