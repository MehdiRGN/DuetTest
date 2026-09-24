using UnityEngine;

// Builds simple placeholder sprites (filled circle / rectangle) at runtime so the
// project has no dependency on imported art assets. Swap these out for real art
// later by assigning a sprite directly on the SpriteRenderer in-editor.
public static class RuntimeSpriteFactory
{
    private const float PixelsPerUnit = 100f;

    public static Sprite CreateCircle(Color color, int diameterPixels = 64)
    {
        var texture = new Texture2D(diameterPixels, diameterPixels, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        float radius = diameterPixels / 2f;
        Vector2 center = new Vector2(radius, radius);

        var pixels = new Color32[diameterPixels * diameterPixels];
        for (int y = 0; y < diameterPixels; y++)
        {
            for (int x = 0; x < diameterPixels; x++)
            {
                float dist = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), center);
                pixels[y * diameterPixels + x] = dist <= radius ? (Color32)color : new Color32(0, 0, 0, 0);
            }
        }

        texture.SetPixels32(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, diameterPixels, diameterPixels), new Vector2(0.5f, 0.5f), PixelsPerUnit);
    }

    public static Sprite CreateRectangle(Color color, int widthPixels = 64, int heightPixels = 64)
    {
        var texture = new Texture2D(widthPixels, heightPixels, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;

        var pixels = new Color32[widthPixels * heightPixels];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels32(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, widthPixels, heightPixels), new Vector2(0.5f, 0.5f), PixelsPerUnit);
    }
}
