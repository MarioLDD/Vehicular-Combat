using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMove : MonoBehaviour
{
    [SerializeField] private GameObject center;
    [SerializeField] private float angle = 10;
    [SerializeField] private int fireDamage;
    [SerializeField] private GameObject currentPlayer;

    void Update()
    {
        transform.RotateAround(center.transform.position, Vector3.up, angle * Time.deltaTime);
    }

    //void OnParticleCollision(GameObject other)
    //{
    //    if (other.TryGetComponentInParents(out HealthSystem healthSystem))
    //    {
    //        if (healthSystem.gameObject != currentPlayer)
    //        {
    //            healthSystem.TakeDamage(fireDamage, currentPlayer);
    //        }
    //    }
    //    Debug.Log(other.name);
    //}
}