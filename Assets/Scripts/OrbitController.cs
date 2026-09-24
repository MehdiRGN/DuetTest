using UnityEngine;

// Rotates two child balls around a shared pivot, Duet-style.
// Tap/hold one side of the screen to rotate clockwise, the other side counter-clockwise.
public class OrbitController : MonoBehaviour
{
    [SerializeField] private float rotationSpeedDegreesPerSecond = 260f;
    [SerializeField] private GameManager gameManager;

    private float currentAngle;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsGameOver)
        {
            return;
        }

        float input = ReadRotationInput();
        if (Mathf.Abs(input) > Mathf.Epsilon)
        {
            currentAngle += input * rotationSpeedDegreesPerSecond * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }

    // Returns -1, 0, or 1 depending on which half of the screen is being pressed.
    private float ReadRotationInput()
    {
        if (Input.GetMouseButton(0))
        {
            return Input.mousePosition.x < Screen.width * 0.5f ? 1f : -1f;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                return touch.position.x < Screen.width * 0.5f ? 1f : -1f;
            }
        }

        return 0f;
    }
}
