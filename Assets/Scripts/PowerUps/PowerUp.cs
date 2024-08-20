using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public abstract string powerUp_Id { get; }
    public abstract void PowerUpSetting(PlayerManager player,float powerUpTime =0f);
    public abstract void Activate();
    public abstract void Deactivate();
}