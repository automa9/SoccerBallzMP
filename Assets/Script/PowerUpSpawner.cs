using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public float spawnInterval = 60f; // Spawn once every 60 seconds
    public float spawnRadius = 10f; // Maximum distance from the spawner position to spawn the power-up

    private void Start()
    {
        InvokeRepeating("SpawnPowerUp", spawnInterval, spawnInterval);
    }

    void SpawnPowerUp()
    {
        Vector3 spawnPosition = GenerateRandomSpawnPosition();
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition += Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = 0f; // Ensure the power-up spawns at ground level or desired height
        return spawnPosition;
    }
}
