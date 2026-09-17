using UnityEngine;

public class StackManager2D : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject blockPrefab2D;
    [SerializeField] private ScoreManager2D scoreManager;
    [SerializeField] private CameraController2D cameraController;

    [Header("Configurações")]
    [SerializeField] private float tolerance = 0.1f; // Margem para acerto perfeito

    private GameObject currentBlock;
    private GameObject lastBlock;

    private bool startFromLeft = true; // Alterna a origem do bloco a cada rodada

    void Start()
    {
        // Instancia a primeira base estática (Geralmente no centro inferior da tela)
        lastBlock = Instantiate(blockPrefab2D, Vector3.zero, Quaternion.identity);
        SpawnNewBlock();
    }

    void Update()
    {
        // Input do jogador (Toque na tela ou clique do mouse)
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }
    }

    private void SpawnNewBlock()
    {
        // A torre cresce no eixo Y somando a altura do bloco anterior
        float newYPosition = lastBlock.transform.position.y + lastBlock.transform.localScale.y;

        currentBlock = Instantiate(blockPrefab2D, new Vector3(0, newYPosition, 0), Quaternion.identity);

        // O novo bloco herda EXATAMENTE o tamanho (escala) que a base atual tem
        currentBlock.transform.localScale = lastBlock.transform.localScale;

        // Adiciona e inicia o script de movimento
        BlockMovement2D mover = currentBlock.AddComponent<BlockMovement2D>();
        mover.Initialize(startFromLeft);
    }

    private void PlaceBlock()
    {
        BlockMovement2D mover = currentBlock.GetComponent<BlockMovement2D>();
        if (mover != null) mover.StopMoving();

        // Distância entre o centro do bloco atual e a base
        float hangover = currentBlock.transform.position.x - lastBlock.transform.position.x;
        float absHangover = Mathf.Abs(hangover);

        // CHECAGEM DE ACERTO PERFEITO
        if (absHangover <= tolerance)
        {
            Vector3 perfectPos = currentBlock.transform.position;
            perfectPos.x = lastBlock.transform.position.x; // Centraliza milimetricamente
            currentBlock.transform.position = perfectPos;

            scoreManager.AddPoint();
            NextTurn();
            return;
        }

        // CÁLCULO DE CORTE (Redução de base)
        float currentWidth = lastBlock.transform.localScale.x;
        float newWidth = currentWidth - absHangover;

        // Se o bloco ficou totalmente para fora da base, Game Over
        if (newWidth <= 0)
        {
            GameOver();
            return;
        }

        // AJUSTE DA NOVA BASE
        float direction = hangover > 0 ? 1f : -1f; // Define se a sobra ficou na direita ou esquerda
        float newXPosition = lastBlock.transform.position.x + (hangover / 2f);

        currentBlock.transform.localScale = new Vector2(newWidth, currentBlock.transform.localScale.y);
        currentBlock.transform.position = new Vector3(newXPosition, currentBlock.transform.position.y, 0);

        // Cria e solta a sobra do bloco que foi cortada
        CreateFallingPiece(newWidth, currentWidth, direction);

        scoreManager.AddPoint();
        NextTurn();
    }

    private void CreateFallingPiece(float newWidth, float oldWidth, float direction)
    {
        // Cria um clone do bloco para ser a sobra
        GameObject fallingPiece = Instantiate(blockPrefab2D);

        // Remove qualquer script de movimento residual
        Destroy(fallingPiece.GetComponent<BlockMovement2D>());

        float fallingWidth = oldWidth - newWidth;
        fallingPiece.transform.localScale = new Vector2(fallingWidth, currentBlock.transform.localScale.y);

        // Posiciona a sobra exatamente ao lado da parte que ficou retida na torre
        float edge = currentBlock.transform.position.x + (newWidth / 2f * direction);
        float fallingXPos = edge + (fallingWidth / 2f * direction);

        fallingPiece.transform.position = new Vector3(fallingXPos, currentBlock.transform.position.y, 0);

        // Adiciona física 2D para ele cair e destrói após 3 segundos
        fallingPiece.AddComponent<Rigidbody2D>();
        Destroy(fallingPiece, 3f);
    }

    private void NextTurn()
    {
        lastBlock = currentBlock;

        // Notifica a câmera para acompanhar a nova altura da torre
        if (cameraController != null)
        {
            cameraController.SetTarget(lastBlock.transform);
        }

        startFromLeft = !startFromLeft;
        SpawnNewBlock();
    }

    private void GameOver()
    {
        // Faz o bloco atual cair se errar feio
        if (currentBlock.GetComponent<Rigidbody2D>() == null)
            currentBlock.AddComponent<Rigidbody2D>();

        Debug.Log("GAME OVER! Pontos: " + scoreManager.GetScore());
    }
}