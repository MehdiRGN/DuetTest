using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float baseSpawnInterval = 1.4f;
    [SerializeField] private float minSpawnInterval = 0.6f;

    private float timeUntilNextSpawn;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    // Used by code-driven scene setup (see GameBootstrapper) to wire this
    // spawner up without needing Editor-assigned serialized fields.
    public void Configure(GameManager manager, Transform spawnPointTransform, GameObject[] prefabs)
    {
        gameManager = manager;
        spawnPoint = spawnPointTransform;
        obstaclePrefabs = prefabs;
    }

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
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0 || spawnPoint == null)
        {
            return;
        }

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject instance = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        instance.SetActive(true);
    }
}
