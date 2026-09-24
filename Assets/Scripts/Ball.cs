using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private BallColor ballColor;
    [SerializeField] private GameManager gameManager;

    public BallColor Color => ballColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var obstacle = other.GetComponent<Obstacle>();
        if (obstacle == null)
        {
            return;
        }

        if (!obstacle.AllowsColor(ballColor))
        {
            gameManager.OnPlayerHit();
        }
    }
}
