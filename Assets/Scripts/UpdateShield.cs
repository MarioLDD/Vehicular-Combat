using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateShield : MonoBehaviour, IUpdateHealth
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material material;
    [SerializeField] private Gradient gradient;
    private float target;
    

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
       // material.color = gradient.Evaluate(0);
    }

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
        target = Mathf.Lerp(1f, 0f, target);
        material.color = gradient.Evaluate(target);
        if (target >= 0.75f)
        {
            float fillValue = Mathf.Lerp(0f, -1f, (target - 0.75f) / 0.25f);
            material.SetFloat("_fill", fillValue);
        }
        else
        {
            material.SetFloat("_fill", 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
       
    }
}
