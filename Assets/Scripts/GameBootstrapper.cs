using UnityEngine;
using UnityEngine.UI;

// Drop this on a single empty GameObject in an otherwise empty scene and hit
// Play: it builds the entire playable scene in code (camera, player, obstacle
// spawner, HUD). This avoids hand-built scene/prefab assets and keeps the
// whole game reviewable as plain C#. Once the game feel is right, this can be
// replaced with hand-authored scene/prefab assets built in the Editor.
public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private float orbitRadius = 1.2f;
    [SerializeField] private float ballRadius = 0.35f;
    [SerializeField] private float obstacleWidth = 6f;
    [SerializeField] private float obstacleHeight = 0.6f;
    [SerializeField] private float gapBarGapWidth = 1.6f;

    private void Awake()
    {
        SetUpCamera();

        var gameManager = new GameObject("GameManager").AddComponent<GameManager>();

        BuildPlayer(gameManager);

        Transform spawnPoint = new GameObject("ObstacleSpawnPoint").transform;
        spawnPoint.position = new Vector3(0f, 6.5f, 0f);

        GameObject gapBarTemplate = BuildGapBarTemplate();
        GameObject colorGateTemplate = BuildColorGateTemplate();

        var spawner = new GameObject("ObstacleSpawner").AddComponent<ObstacleSpawner>();
        spawner.Configure(gameManager, spawnPoint, new[] { gapBarTemplate, colorGateTemplate });

        BuildHud(gameManager);
    }

    private void SetUpCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            cam = new GameObject("Main Camera").AddComponent<Camera>();
            cam.tag = "MainCamera";
        }

        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.backgroundColor = new Color(0.08f, 0.08f, 0.1f);
        cam.transform.position = new Vector3(0f, 0f, -10f);
    }

    private void BuildPlayer(GameManager gameManager)
    {
        var pivot = new GameObject("PlayerPivot");
        pivot.transform.position = new Vector3(0f, -2f, 0f);
        var orbit = pivot.AddComponent<OrbitController>();
        orbit.SetGameManager(gameManager);

        CreateBall(pivot.transform, BallColor.A, new Vector3(-orbitRadius, 0f, 0f), gameManager);
        CreateBall(pivot.transform, BallColor.B, new Vector3(orbitRadius, 0f, 0f), gameManager);
    }

    private void CreateBall(Transform parent, BallColor color, Vector3 localPosition, GameManager gameManager)
    {
        var ballObject = new GameObject($"Ball{color}");
        ballObject.transform.SetParent(parent);
        ballObject.transform.localPosition = localPosition;

        var spriteRenderer = ballObject.AddComponent<SpriteRenderer>();
        var collider = ballObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = ballRadius;

        var rigidbody2d = ballObject.AddComponent<Rigidbody2D>();
        rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2d.gravityScale = 0f;

        var ball = ballObject.AddComponent<Ball>();
        ball.Configure(color, gameManager);
        _ = spriteRenderer;
    }

    private GameObject BuildGapBarTemplate()
    {
        var root = new GameObject("Obstacle_GapBar");
        var mover = root.AddComponent<ObstacleMover>();
        _ = mover;

        float sideWidth = (obstacleWidth - gapBarGapWidth) / 2f;
        float leftCenterX = -(gapBarGapWidth / 2f) - (sideWidth / 2f);
        float rightCenterX = (gapBarGapWidth / 2f) + (sideWidth / 2f);

        CreateObstaclePiece(root.transform, "Left", new Vector3(leftCenterX, 0f, 0f), new Vector2(sideWidth, obstacleHeight), true, BallColor.A);
        CreateObstaclePiece(root.transform, "Right", new Vector3(rightCenterX, 0f, 0f), new Vector2(sideWidth, obstacleHeight), true, BallColor.A);

        root.SetActive(false);
        return root;
    }

    private GameObject BuildColorGateTemplate()
    {
        var root = new GameObject("Obstacle_ColorGate");
        var mover = root.AddComponent<ObstacleMover>();
        _ = mover;

        float halfWidth = obstacleWidth / 2f;
        CreateObstaclePiece(root.transform, "Left", new Vector3(-halfWidth / 2f, 0f, 0f), new Vector2(halfWidth, obstacleHeight), false, BallColor.A);
        CreateObstaclePiece(root.transform, "Right", new Vector3(halfWidth / 2f, 0f, 0f), new Vector2(halfWidth, obstacleHeight), false, BallColor.B);

        root.SetActive(false);
        return root;
    }

    private void CreateObstaclePiece(Transform parent, string name, Vector3 localPosition, Vector2 size, bool blocksAll, BallColor passableColor)
    {
        var piece = new GameObject(name);
        piece.transform.SetParent(parent);
        piece.transform.localPosition = localPosition;

        piece.AddComponent<SpriteRenderer>();
        piece.AddComponent<BoxCollider2D>();

        var obstacle = piece.AddComponent<Obstacle>();
        obstacle.Configure(blocksAll, passableColor, size);
    }

    private void BuildHud(GameManager gameManager)
    {
        var canvasObject = new GameObject("Canvas");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        canvasObject.AddComponent<GraphicRaycaster>();

        if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        Text scoreText = CreateLabel(canvasObject.transform, "ScoreText", 64, TextAnchor.UpperCenter, new Vector2(0f, -80f));

        var gameOverPanel = new GameObject("GameOverPanel");
        gameOverPanel.transform.SetParent(canvasObject.transform, false);
        var panelRect = gameOverPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        var panelImage = gameOverPanel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.6f);
        gameOverPanel.SetActive(false);

        CreateLabel(gameOverPanel.transform, "GameOverText", 96, TextAnchor.MiddleCenter, Vector2.zero, "GAME OVER");
        CreateLabel(gameOverPanel.transform, "RestartHintText", 48, TextAnchor.MiddleCenter, new Vector2(0f, -140f), "Tap to restart");

        var hud = canvasObject.AddComponent<HudController>();
        hud.Configure(gameManager, scoreText, gameOverPanel);
    }

    private Text CreateLabel(Transform parent, string name, int fontSize, TextAnchor anchor, Vector2 anchoredPosition, string initialText = "0")
    {
        var textObject = new GameObject(name);
        textObject.transform.SetParent(parent, false);

        var rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(900f, 200f);
        rectTransform.anchoredPosition = anchoredPosition;

        var text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.alignment = anchor;
        text.color = Color.white;
        text.text = initialText;

        return text;
    }
}
