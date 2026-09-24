using UnityEngine;

// Moves an obstacle downward at a constant speed and destroys it once
// it passes below the camera's view, driven by GameManager's difficulty curve.
public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float destroyYPosition = -12f;

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
        {
            return;
        }

        float speed = gameManager != null ? gameManager.CurrentObstacleSpeed : 5f;
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y < destroyYPosition)
        {
            Destroy(gameObject);
        }
    }
}
