using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShieldSystem : MonoBehaviour
{

    [SerializeField] private GameObject shield;
    private MeshRenderer meshRenderer;
    private Material material;

    [SerializeField] private Image shield_image;
    [SerializeField] private TMP_Text shield_Text;
    private bool ready;
    private HealthSystem healthSystem;

    
    // Start is called before the first frame update
    void Start()
    {
        healthSystem = shield.GetComponent<HealthSystem>();
        meshRenderer = shield.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        DisableShield();
    }
    public void PrepareShield(int shieldHealth)
    {
        healthSystem.MaxHealth = shieldHealth;
        healthSystem.CurrentHealth = shieldHealth;
        if(healthSystem.gameObject.activeSelf)
        {
            healthSystem.UpdateVisual();
            material.SetFloat("_fill", 0.02f);
            return;
        }
        shield_image.color = Color.white;
        shield_Text.color = Color.white;
        ready = true;
        healthSystem.UpdateVisual();
        material.SetFloat("_fill", 0.02f);
    }
    public void DisableShield()
    {
        shield.SetActive(false);
        shield_image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        shield_Text.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }
    public void OnShield()
    {
        if (ready)
        {
            shield.SetActive(true);
            ready = false;
        }
    }
}