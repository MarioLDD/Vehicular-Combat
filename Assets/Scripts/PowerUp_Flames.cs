using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Flames : MonoBehaviour
{
    [SerializeField] private PowerUpFactory powerUpFactory;

    [SerializeField] private int particleLiveTime;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if (other.gameObject.TryGetComponentInParents(out PlayerManager playerManager))
            {

                //switch (playerManager.PowerUp?.GetType())
                //{
                //    case typeof(PowerUp):


                //        break;
                //    case typeof(PowerUp_Flames):


                //        break;
                //    default:


                //        break;
                //}







                //meter un switch para ver si el del playermanager en null, igual o distinto. si son distintos tengo que cortar la ejecucion del actual sin que borre la referncia
                if (playerManager.PowerUp?.GetType() != powerUpFactory.GetPowerUpPrefabById("Fire").GetType())
                {
                    PowerUp objectInstance = powerUpFactory.Create("Fire", playerManager.PowerUpTrailPosition);
                    playerManager.PowerUp = objectInstance;

                    objectInstance.PowerUpSetting(playerManager, particleLiveTime);
                }
                else
                {
                    playerManager.PowerUp.PowerUpSetting(playerManager, particleLiveTime);
                }
            }
            //other.GetComponentInParent<FlamesSystem>().PreparePowerUp(particleLiveTime);
            Destroy(gameObject);
        }
    }



}