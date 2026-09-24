using UnityEngine;

// A single obstacle piece. If blocksAllColors is false, balls matching
// passableColor pass through safely while the other color dies on contact.
public class Obstacle : MonoBehaviour
{
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
}
