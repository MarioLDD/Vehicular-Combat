using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerupInfo
{
    public GameObject powerupPrefab;
    public float spawnRatio;
}

public class PowerupSpawner : MonoBehaviour
{
    public List<PowerupInfo> powerupList;
    public float spawnRadius = 10f;
    public float spawnHeight = 5f;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;

    private float nextSpawnTime;

    private void Start()
    {
        // Calcula el tiempo de spawn inicial.
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomPowerup();
            // Calcula el tiempo de spawn para el próximo powerup.
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnRandomPowerup()
    {
        float totalRatio = 0f;
        foreach (var powerupInfo in powerupList)
        {
            totalRatio += powerupInfo.spawnRatio;
        }

        float randomValue = Random.Range(0f, totalRatio);

        GameObject selectedPowerup = null;
        foreach (var powerupInfo in powerupList)
        {
            if (randomValue <= powerupInfo.spawnRatio)
            {
                selectedPowerup = powerupInfo.powerupPrefab;
                break;
            }
            randomValue -= powerupInfo.spawnRatio;
        }

        if (selectedPowerup != null)
        {
            // Calcula una posición aleatoria en el radio especificado.
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), spawnHeight, Random.Range(-spawnRadius, spawnRadius));

            // Instancia el powerup en la posición calculada.
            Instantiate(selectedPowerup, spawnPosition, Quaternion.identity);
        }
    }
}
