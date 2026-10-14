using UnityEngine;

public class StackManager2D : MonoBehaviour
{
    [Header("Referências da UI")]
    [SerializeField] private ScoreManager2D scoreManager;
    [SerializeField] public GameObject restartPanel;
    [SerializeField] private CameraController2D cameraController;

    [Header("Efeitos Visuais (SOLID)")]
    [SerializeReference] private ITowerEffectService effectService = new ParticleEffectService();

    [Header("Configurações do Jogo")]
    [SerializeField] private GameObject blockPrefab2D;
    [SerializeField] private float tolerance = 0.1f;

    [Header("Mecânica de Combo (Tamanho Inicial / Restauração)")]
    [Tooltip("Número de acertos perfeitos consecutivos necessários para restaurar a escala original do bloco")]
    [SerializeField] private int perfectComboTarget = 5;

    private GameObject currentBlock;
    private GameObject lastBlock;
    private bool startFromLeft = true;

    private int consecutivePerfectHits = 0;
    private float originalBlockWidth;

    void Start()
    {
        originalBlockWidth = blockPrefab2D.transform.localScale.x;

        lastBlock = Instantiate(blockPrefab2D, Vector3.zero, Quaternion.identity);
        SpawnNewBlock();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }
    }

    private void SpawnNewBlock()
    {
        float newYPosition = lastBlock.transform.position.y + lastBlock.transform.localScale.y;

        currentBlock = Instantiate(blockPrefab2D, new Vector3(0, newYPosition, 0), Quaternion.identity);
        currentBlock.transform.localScale = lastBlock.transform.localScale;

        BlockMovement2D mover = currentBlock.AddComponent<BlockMovement2D>();
        mover.Initialize(startFromLeft);
    }

    private void PlaceBlock()
    {
        BlockMovement2D mover = currentBlock.GetComponent<BlockMovement2D>();
        if (mover != null) mover.StopMoving();

        float hangover = currentBlock.transform.position.x - lastBlock.transform.position.x;
        float absHangover = Mathf.Abs(hangover);

        if (absHangover <= tolerance)
        {
            Vector3 perfectPos = currentBlock.transform.position;
            perfectPos.x = lastBlock.transform.position.x;
            currentBlock.transform.position = perfectPos;

            consecutivePerfectHits++;
            CheckAndApplyComboBonus();

            // Toca o efeito de partículas na posição do acerto perfeito
            effectService?.PlayPlacementEffect(currentBlock.transform.position);

            scoreManager.AddPoint();
            NextTurn();
            return;
        }

        consecutivePerfectHits = 0;

        float currentWidth = lastBlock.transform.localScale.x;
        float newWidth = currentWidth - absHangover;

        if (newWidth <= 0)
        {
            GameOver();
            return;
        }

        float direction = hangover > 0 ? 1f : -1f;
        float newXPosition = lastBlock.transform.position.x + (hangover / 2f);

        currentBlock.transform.localScale = new Vector2(newWidth, currentBlock.transform.localScale.y);
        currentBlock.transform.position = new Vector3(newXPosition, currentBlock.transform.position.y, 0);

        CreateFallingPiece(newWidth, currentWidth, direction);

        // Toca o efeito de partículas na posição do bloco encaixado
        effectService?.PlayPlacementEffect(currentBlock.transform.position);

        scoreManager.AddPoint();
        NextTurn();
    }

    private void CheckAndApplyComboBonus()
    {
        if (consecutivePerfectHits >= perfectComboTarget)
        {
            Vector3 restoredScale = currentBlock.transform.localScale;
            restoredScale.x = originalBlockWidth;
            currentBlock.transform.localScale = restoredScale;

            Debug.Log($"<color=green>COMBO PERFEITO ALCANÇADO ({consecutivePerfectHits})!</color> Escala do bloco restaurada para {originalBlockWidth}.");

            consecutivePerfectHits = 0;
        }
    }

    private void CreateFallingPiece(float newWidth, float oldWidth, float direction)
    {
        GameObject fallingPiece = Instantiate(blockPrefab2D);
        Destroy(fallingPiece.GetComponent<BlockMovement2D>());

        float fallingWidth = oldWidth - newWidth;
        fallingPiece.transform.localScale = new Vector2(fallingWidth, currentBlock.transform.localScale.y);

        float edge = currentBlock.transform.position.x + (newWidth / 2f * direction);
        float fallingXPos = edge + (fallingWidth / 2f * direction);

        fallingPiece.transform.position = new Vector3(fallingXPos, currentBlock.transform.position.y, 0);

        fallingPiece.AddComponent<Rigidbody2D>();
        Destroy(fallingPiece, 3f);
    }

    private void NextTurn()
    {
        lastBlock = currentBlock;

        if (cameraController != null)
        {
            cameraController.SetTarget(lastBlock.transform);
        }

        startFromLeft = !startFromLeft;
        SpawnNewBlock();
    }

    private void GameOver()
    {
        BlockMovement2D mover = currentBlock.GetComponent<BlockMovement2D>();
        if (mover != null) mover.StopMoving();

        if (currentBlock.GetComponent<Rigidbody2D>() == null)
            currentBlock.AddComponent<Rigidbody2D>();

        if (restartPanel != null)
            restartPanel.SetActive(true);

        if (cameraController != null && lastBlock != null)
        {
            float finalTowerHeight = lastBlock.transform.position.y;
            cameraController.ZoomOutToOverview(finalTowerHeight);
        }

        Debug.Log("GAME OVER! Pontos: " + scoreManager.GetScore());
    }
}