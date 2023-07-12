using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    public GameObject powerUpPrefab;
    public float spawnInterval = 60f;
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);
    }

    void SpawnPowerUp()
    {
        if(!isSpawning)
        {
            isSpawning = true;
            Vector3 spawnPosition = transform.position;
            Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
            isSpawning = false;
        }
    }
}
