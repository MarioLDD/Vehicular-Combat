using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlamesSystem : MonoBehaviour
{
    [SerializeField] private GameObject flamesPoint;
    [SerializeField] private ParticleSystem wildFire;
    [SerializeField] private Image fire_image;
    private int particleLiveTime;
    private bool ready;


    void Start()
    {
        if (flamesPoint == null)
        {
            flamesPoint = gameObject.GetComponent<PlayerManager>().FlamesPoint;
        }
        if (wildFire == null)
        {
            wildFire = flamesPoint.GetComponentInChildren<ParticleSystem>();
        }
        DisableFireIcon();
    }

    public void PreparePowerUp(int LiveTime)
    {
        particleLiveTime = LiveTime;
        fire_image.color = Color.white;
        ready = true;
    }

    private void DisableFireIcon()
    {
        fire_image.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    private IEnumerator FireActivator()
    {
        wildFire.Play();
        yield return new WaitForSeconds(particleLiveTime);
        wildFire.Stop();
        DisableFireIcon();
    }

    public void OnPowerUps()
    {
        if (ready)
        {
            StopCoroutine(FireActivator());
            StartCoroutine(FireActivator());
            ready = false;
        }
    }
}