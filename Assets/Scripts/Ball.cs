using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Ball : MonoBehaviour
{
    private static readonly Color ColorAVisual = new Color(0.22f, 0.78f, 0.93f);
    private static readonly Color ColorBVisual = new Color(0.96f, 0.32f, 0.55f);

    [SerializeField] private BallColor ballColor;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public BallColor Color => ballColor;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        ApplyVisual();
    }

    // Used by code-driven scene setup (see GameBootstrapper) to configure a
    // ball without needing Editor-assigned serialized fields.
    public void Configure(BallColor color, GameManager manager)
    {
        ballColor = color;
        gameManager = manager;
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = RuntimeSpriteFactory.CreateCircle(ballColor == BallColor.A ? ColorAVisual : ColorBVisual);
        }
    }

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
