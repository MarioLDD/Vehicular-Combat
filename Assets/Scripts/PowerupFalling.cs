using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupFalling : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass"))
        {
            // Cambiar el Collider a Trigger cuando el powerup toca el suelo.
            GetComponent<Collider>().isTrigger = true;
            // Detener la física del Rigidbody.
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
