using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour, IUpdateHealth
{
    [SerializeField] private Image healthBarSprite_P1;
    [SerializeField] private Image healthBarSprite_P2;
    [SerializeField] private Image healthBarSprite_P3;
    [SerializeField] private Image healthBarSprite_P4;
    [SerializeField] private Transform healthBarCanvas_P1;
    [SerializeField] private Transform healthBarCanvas_P2;
    [SerializeField] private Transform healthBarCanvas_P3;
    [SerializeField] private Transform healthBarCanvas_P4;
    [SerializeField] private Camera camera_P1;
    [SerializeField] private Camera camera_P2;
    [SerializeField] private Camera camera_P3;
    [SerializeField] private Camera camera_P4;
    [SerializeField] private float reduceSpeed = 2;
    private float target;

    private void Start()
    {
        target = 1;
    }
    private void Update()
    {
        if (camera_P1 != null)
        {
            //Debug.Log(target);
            healthBarCanvas_P1.rotation = Quaternion.LookRotation(transform.position - camera_P1.transform.position);
           healthBarSprite_P1.fillAmount = Mathf.MoveTowards(healthBarSprite_P1.fillAmount, target, reduceSpeed * Time.deltaTime);
        }
        if (camera_P2 != null)
        {

            healthBarCanvas_P2.rotation = Quaternion.LookRotation(transform.position - camera_P2.transform.position);
            healthBarSprite_P2.fillAmount = Mathf.MoveTowards(healthBarSprite_P2.fillAmount, target, reduceSpeed * Time.deltaTime);
        }
        if (camera_P3 != null)
        {

            healthBarCanvas_P3.rotation = Quaternion.LookRotation(transform.position - camera_P3.transform.position);
            healthBarSprite_P3.fillAmount = Mathf.MoveTowards(healthBarSprite_P3.fillAmount, target, reduceSpeed * Time.deltaTime);
        }
        if (camera_P4 != null)
        {

            healthBarCanvas_P4.rotation = Quaternion.LookRotation(transform.position - camera_P4.transform.position);
            healthBarSprite_P4.fillAmount = Mathf.MoveTowards(healthBarSprite_P4.fillAmount, target, reduceSpeed * Time.deltaTime);
        }
    }

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;        
    }
}
