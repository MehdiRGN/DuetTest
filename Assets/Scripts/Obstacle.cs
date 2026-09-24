using UnityEngine;

// A single obstacle piece. If blocksAllColors is false, balls matching
// passableColor pass through safely while the other color dies on contact.
[RequireComponent(typeof(SpriteRenderer))]
public class Obstacle : MonoBehaviour
{
    private static readonly Color BlockingVisual = new Color(0.85f, 0.85f, 0.85f);
    private static readonly Color ColorAVisual = new Color(0.22f, 0.78f, 0.93f);
    private static readonly Color ColorBVisual = new Color(0.96f, 0.32f, 0.55f);

    [SerializeField] private bool blocksAllColors;
    [SerializeField] private BallColor passableColor;

    public bool AllowsColor(BallColor ballColor)
    {
        if (blocksAllColors)
        {
            return false;
        }

        return ballColor == passableColor;
    }

    // Used by code-driven scene setup (see GameBootstrapper) to configure and
    // visually size a piece without needing Editor-assigned serialized fields.
    public void Configure(bool blocksAll, BallColor allowedColor, Vector2 sizeUnits)
    {
        blocksAllColors = blocksAll;
        passableColor = allowedColor;

        var renderer = GetComponent<SpriteRenderer>();
        int pixelsPerUnit = 100;
        renderer.sprite = RuntimeSpriteFactory.CreateRectangle(
            blocksAllColors ? BlockingVisual : (passableColor == BallColor.A ? ColorAVisual : ColorBVisual),
            Mathf.Max(1, Mathf.RoundToInt(sizeUnits.x * pixelsPerUnit)),
            Mathf.Max(1, Mathf.RoundToInt(sizeUnits.y * pixelsPerUnit)));

        var collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = sizeUnits;
            collider.isTrigger = true;
        }
    }
}
