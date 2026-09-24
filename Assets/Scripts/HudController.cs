using UnityEngine;
using UnityEngine.UI;

// Drives the score label and game-over panel from GameManager state.
// Tap/click anywhere while the game-over panel is showing to restart.
public class HudController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameOverPanel;

    public void Configure(GameManager manager, Text score, GameObject panel)
    {
        gameManager = manager;
        scoreText = score;
        gameOverPanel = panel;
    }

    private void Update()
    {
        if (gameManager == null)
        {
            return;
        }

        if (scoreText != null)
        {
            scoreText.text = gameManager.Score.ToString();
        }

        if (gameOverPanel != null && gameOverPanel.activeSelf != gameManager.IsGameOver)
        {
            gameOverPanel.SetActive(gameManager.IsGameOver);
        }

        if (gameManager.IsGameOver && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            gameManager.RestartLevel();
        }
    }
}
