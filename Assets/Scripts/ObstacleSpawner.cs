using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float baseSpawnInterval = 1.4f;
    [SerializeField] private float minSpawnInterval = 0.6f;

    private float timeUntilNextSpawn;

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
        {
            return;
        }

        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn <= 0f)
        {
            SpawnObstacle();
            float interval = baseSpawnInterval - (gameManager != null ? gameManager.DifficultyElapsedTime * 0.01f : 0f);
            timeUntilNextSpawn = Mathf.Max(minSpawnInterval, interval);
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            return;
        }

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
