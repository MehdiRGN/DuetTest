using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float startingObstacleSpeed = 5f;
    [SerializeField] private float maxObstacleSpeed = 12f;
    [SerializeField] private float speedRampPerSecond = 0.05f;

    public bool IsGameOver { get; private set; }
    public float DifficultyElapsedTime { get; private set; }
    public float CurrentObstacleSpeed { get; private set; }
    public int Score { get; private set; }

    private void Awake()
    {
        CurrentObstacleSpeed = startingObstacleSpeed;
    }

    private void Update()
    {
        if (IsGameOver)
        {
            return;
        }

        DifficultyElapsedTime += Time.deltaTime;
        CurrentObstacleSpeed = Mathf.Min(maxObstacleSpeed, startingObstacleSpeed + DifficultyElapsedTime * speedRampPerSecond);
        Score = Mathf.FloorToInt(DifficultyElapsedTime * 10f);
    }

    public void OnPlayerHit()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
