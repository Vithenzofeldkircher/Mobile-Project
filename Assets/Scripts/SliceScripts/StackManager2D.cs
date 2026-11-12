using UnityEngine;
using UnityEngine.EventSystems;

public class StackManager2D : MonoBehaviour
{
    [Header("Referências da UI")]
    [SerializeField] private ScoreManager2D scoreManager;
    [SerializeField] public GameObject restartPanel;
    [SerializeField] private CameraController2D cameraController;

    [Header("Efeitos Visuais (SOLID)")]
    [SerializeReference] private ITowerEffectService effectService = new ParticleEffectService();

    [Header("Configurações do Jogo")]
    [SerializeField] private GameObject[] blockPrefab2D;
    [SerializeField] private float tolerance = 0.1f;

    [Header("Mecânica de Combo (Tamanho Inicial / Restauração)")]
    [Tooltip("Número de acertos perfeitos consecutivos necessários para restaurar a escala original do bloco")]
    [SerializeField] private int perfectComboTarget = 5;

    private GameObject currentBlock;
    private GameObject lastBlock;
    private bool startFromLeft = true;

    private int consecutivePerfectHits = 0;
    private int currentPrefabIndex = 0;
    private float currentOriginalWidth;

    void Start()
    {
        // Reseta e inicializa os estados dos upgrades desta partida
        DoubleScoreManager.ResetRunState();
        StopWindRunManager.ResetRun();
        BlockCycleRunManager.ResetRun();
        BlockCycleRunManager.InitializeRun();

        if (blockPrefab2D == null || blockPrefab2D.Length == 0)
        {
            Debug.LogError("Nenhum prefab de bloco foi atribuído no Inspector!");
            return;
        }

        int upgradeBonus = UpgradeManager.GetUpgradeLevel("BlockType");
        int baseIndex = Mathf.FloorToInt(Random.value * blockPrefab2D.Length);
        currentPrefabIndex = (baseIndex + upgradeBonus) % blockPrefab2D.Length;

        currentOriginalWidth = blockPrefab2D[currentPrefabIndex].transform.localScale.x;

        lastBlock = Instantiate(blockPrefab2D[currentPrefabIndex], Vector3.zero, Quaternion.identity);
        SpawnNewBlock();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            PlaceBlock();
        }
    }

    // NOVO MÉTODO: Altera o prefab do bloco atual para o próximo na Array ciclicamente
    public void TryCycleBlock()
    {
        if (!BlockCycleRunManager.HasCharges || currentBlock == null) return;

        // Consome uma carga
        BlockCycleRunManager.ConsumeCharge();

        // Guarda a posição e largura atuais do bloco em movimento
        float currentX = currentBlock.transform.position.x;
        float currentY = currentBlock.transform.position.y;
        float currentWidth = currentBlock.transform.localScale.x;

        // Destroi o bloco atual em movimento
        Destroy(currentBlock);

        // Avança para o próximo índice da Array de forma circular
        currentPrefabIndex = (currentPrefabIndex + 1) % blockPrefab2D.Length;
        currentOriginalWidth = blockPrefab2D[currentPrefabIndex].transform.localScale.x;

        // Instancia o novo bloco com o novo prefab na mesma posição
        currentBlock = Instantiate(blockPrefab2D[currentPrefabIndex], new Vector3(currentX, currentY, 0), Quaternion.identity);

        // Mantém a largura (X) que estava sendo usada
        Vector3 newScale = currentBlock.transform.localScale;
        newScale.x = currentWidth;
        currentBlock.transform.localScale = newScale;

        // Reaplica o movimento
        BlockMovement2D mover = currentBlock.AddComponent<BlockMovement2D>();
        mover.Initialize(startFromLeft);

        Debug.Log($"<color=magenta>Bloco trocado!</color> Novo índice: {currentPrefabIndex}. Cargas restantes: {BlockCycleRunManager.ChargesLeft}");
    }

    private void SpawnNewBlock()
    {
        currentPrefabIndex = Mathf.FloorToInt(Random.value * blockPrefab2D.Length);
        currentPrefabIndex = Mathf.Clamp(currentPrefabIndex, 0, blockPrefab2D.Length - 1);

        currentOriginalWidth = blockPrefab2D[currentPrefabIndex].transform.localScale.x;

        currentBlock = Instantiate(blockPrefab2D[currentPrefabIndex], Vector3.zero, Quaternion.identity);

        Vector3 newScale = currentBlock.transform.localScale;
        newScale.x = lastBlock.transform.localScale.x;
        currentBlock.transform.localScale = newScale;

        float newYPosition = lastBlock.transform.position.y
                           + (lastBlock.transform.localScale.y / 2f)
                           + (currentBlock.transform.localScale.y / 2f);

        currentBlock.transform.position = new Vector3(0, newYPosition, 0);

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

        effectService?.PlayPlacementEffect(currentBlock.transform.position);

        scoreManager.AddPoint();
        NextTurn();
    }

    private void CheckAndApplyComboBonus()
    {
        if (consecutivePerfectHits >= perfectComboTarget)
        {
            Vector3 restoredScale = currentBlock.transform.localScale;
            restoredScale.x = currentOriginalWidth;
            currentBlock.transform.localScale = restoredScale;

            consecutivePerfectHits = 0;
        }
    }

    private void CreateFallingPiece(float newWidth, float oldWidth, float direction)
    {
        GameObject fallingPiece = Instantiate(blockPrefab2D[currentPrefabIndex]);
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
    }
}