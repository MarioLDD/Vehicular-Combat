using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFactory : MonoBehaviour
{
    [SerializeField] private PowerUp[] powerUps;
    private Dictionary<string, PowerUp> powerUpById;


    //private readonly PowerUpConfiguration _powerUpConfiguration;
    private void Awake()
    {
        powerUpById = new Dictionary<string, PowerUp>();
        foreach (var powerUp in powerUps)
        {
            powerUpById.Add(powerUp.powerUp_Id, powerUp);
        }
    }
    public PowerUp GetPowerUpPrefabById(string id)
    {
        if (!powerUpById.TryGetValue(id, out var powerUp))
        {
            throw new Exception($"PowerUp with id {id} does not exit");
        }

        return powerUp;
    }

    public PowerUp Create(string id, Transform transform)
    {
        if (powerUpById.TryGetValue(id, out PowerUp objectPrefab))
        {
            PowerUp objectInstance = Instantiate(objectPrefab, transform);
            return objectInstance;
        }
        else
        {
            Debug.Log(id + " no se encuentra en el diccionario");
            return null;
        }
    }
}
