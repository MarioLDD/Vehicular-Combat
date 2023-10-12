using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Collider playerCollider;
    public UnityEvent onHealthZero;
    private int currentHealth;
    private IUpdateHealth updateHealth;
    private Vector3 spawnPosition;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }

    private void Start()
    {
        spawnPosition = gameObject.transform.position;
        currentHealth = maxHealth;        
        updateHealth = GetComponentInChildren<IUpdateHealth>(false);
        if (playerCollider == null)
        {
            playerCollider = GetComponentInChildren<Collider>(false);

        }
    }
    public void TakeDamage(int damage, GameObject playerWhoShot = null)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;

            UpdateVisual();


            if (currentHealth < 1)
            {
                onHealthZero.Invoke();
                //if (isPlayer)
                {
                    if (this.gameObject.TryGetComponent(out PlayerManager playerManager))
                    {
                        playerCollider.enabled = false;
                        currentHealth = maxHealth;
                        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                        Instantiate(explosion, rb.worldCenterOfMass, Quaternion.identity);
                        playerCollider.enabled = true;
                        rb.MovePosition(spawnPosition);
                        UpdateVisual();
                        Debug.Log("El " + gameObject.name + " ha sido asesinado por " + playerWhoShot.name);
                        playerManager.LoseScore(playerWhoShot);
                    }
                }
            }
        }
    }

    public void UpdateVisual()
    {
        updateHealth.UpdateHealth(maxHealth, currentHealth);
    }
}