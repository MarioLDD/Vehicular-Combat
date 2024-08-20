using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireDamage : PowerUp
{
    [ReadOnly][SerializeField] private ParticleSystem wildFire;
    [ReadOnly][SerializeField] private float particleLiveTime;
    [SerializeField] private int fireDamage = 2;
    [ReadOnly][SerializeField] private GameObject currentPlayer;

    [SerializeField] private Sprite powerUp_Sprite;
    private PlayerManager _playerManager;
    public override string powerUp_Id => "Fire";

    void Start()
    {
        wildFire = gameObject.GetComponent<ParticleSystem>();
        if (currentPlayer == null)
        {
            currentPlayer = gameObject.transform.parent.gameObject;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collider");

        if (other.TryGetComponentInParents(out HealthSystem healthSystem))
        {
            Debug.Log("Fuego");
            if (healthSystem.gameObject != currentPlayer)
            {
                healthSystem.TakeDamage(fireDamage, currentPlayer);
                var collisionParticle = gameObject.GetComponent<ParticleSystem>().collision;
                collisionParticle.enabled = false;
                //gameObject.GetComponent<ParticleSystem>().
            }
        }
    }
    private IEnumerator FireActivator()
    {
        wildFire.Play();
        yield return new WaitForSeconds(particleLiveTime);
        wildFire.Stop();
    }

    private void OnParticleSystemStopped()
    {
        _playerManager.DisablePowerUp();
    }
    public override void PowerUpSetting(PlayerManager playerManager, float powerUpTime)
    {
        particleLiveTime = powerUpTime;
        _playerManager = playerManager;
        playerManager.PreparePowerUp(powerUp_Sprite);
    }

    public override void Activate()
    {
        StopCoroutine(FireActivator());
        StartCoroutine(FireActivator());
    }

    public override void Deactivate()
    {
        StopCoroutine(FireActivator());
        wildFire.Stop();
    }
}