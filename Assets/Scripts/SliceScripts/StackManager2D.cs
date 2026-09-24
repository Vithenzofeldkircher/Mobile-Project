using UnityEngine;

public class StackManager2D : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject blockPrefab2D;
    [SerializeField] private ScoreManager2D scoreManager;
    [SerializeField] private CameraController2D cameraController;
    [SerializeField] private GameObject panelRestart;

    [Header("Configurações de Regra")]
    [SerializeField] private float tolerance = 0.1f;
    [SerializeField] private int comboParaRestaurar = 5;

    private GameObject currentBlock;
    private GameObject lastBlock;
    private bool startFromLeft = true;

    private float originalBlockWidth;
    private int perfectComboCount = 0;

    void Start()
    {
        // Salva o tamanho original do prefab
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

        // O BlockMovement2D agora recebe o score e calcula a própria velocidade internamente (SOLID)
        BlockMovement2D mover = currentBlock.AddComponent<BlockMovement2D>();
        mover.Initialize(startFromLeft, scoreManager.GetScore());
    }

    private void PlaceBlock()
    {
        BlockMovement2D mover = currentBlock.GetComponent<BlockMovement2D>();
        if (mover != null) mover.StopMoving();

        float hangover = currentBlock.transform.position.x - lastBlock.transform.position.x;
        float absHangover = Mathf.Abs(hangover);

        // CHECAGEM DE ACERTO PERFEITO
        if (absHangover <= tolerance)
        {
            perfectComboCount++;

            // Centraliza o bloco perfeitamente
            Vector3 perfectPos = currentBlock.transform.position;
            perfectPos.x = lastBlock.transform.position.x;
            currentBlock.transform.position = perfectPos;

            // RECOMPENSA: Se atingiu 5 ou mais acertos perfeitos seguidos, restaura o tamanho original
            if (perfectComboCount >= comboParaRestaurar)
            {
                currentBlock.transform.localScale = new Vector2(originalBlockWidth, currentBlock.transform.localScale.y);
                perfectComboCount = 0; // Reseta o contador

                scoreManager.AddPoint();
                NextTurn();
                return; // Encerra a função
            }

            scoreManager.AddPoint();
            NextTurn();
            return;
        }

        // Se errou o ponto perfeito, reseta a contagem de combo
        perfectComboCount = 0;

        // CÁLCULO DE CORTE
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

        scoreManager.AddPoint();
        NextTurn();
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
        if (currentBlock.GetComponent<Rigidbody2D>() == null)
            currentBlock.AddComponent<Rigidbody2D>();

        if (panelRestart != null)
            panelRestart.SetActive(true);
    }
}