using UnityEngine;

public class StackManager : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float tolerance = 0.1f; // Tolerância para acerto perfeito

    private GameObject currentBlock;
    private GameObject lastBlock;

    private bool isMovingOnX = true;
    private int stackCount = 0;

    void Start()
    {
        // Cria a primeira base fixa
        lastBlock = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
        SpawnNewBlock();
    }

    void Update()
    {
        // Detecta o clique do mouse ou toque na tela
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }
    }

    private void SpawnNewBlock()
    {
        // A altura sobe a cada bloco empilhado (baseando-se na escala Y padrão do cubo)
        float newYPosition = lastBlock.transform.position.y + lastBlock.transform.localScale.y;

        Vector3 spawnPosition = isMovingOnX
            ? new Vector3(-3f, newYPosition, lastBlock.transform.position.z)
            : new Vector3(lastBlock.transform.position.x, newYPosition, -3f);

        currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);

        // Copia o tamanho exato da base anterior
        currentBlock.transform.localScale = lastBlock.transform.localScale;

        MovingBlock mover = currentBlock.AddComponent<MovingBlock>();
        mover.Initialize(isMovingOnX);
    }

    private void PlaceBlock()
    {
        MovingBlock mover = currentBlock.GetComponent<MovingBlock>();
        mover.StopMoving();

        float hangover = isMovingOnX
            ? currentBlock.transform.position.x - lastBlock.transform.position.x
            : currentBlock.transform.position.z - lastBlock.transform.position.z;

        // Se o jogador acertou dentro da tolerância, centraliza perfeitamente
        if (Mathf.Abs(hangover) <= tolerance)
        {
            Vector3 perfectPos = currentBlock.transform.position;
            if (isMovingOnX) perfectPos.x = lastBlock.transform.position.x;
            else perfectPos.z = lastBlock.transform.position.z;

            currentBlock.transform.position = perfectPos;

            scoreManager.AddPoint();
            NextTurn();
            return;
        }

        // Calcula qual parte do bloco ficou para fora
        float currentSize = isMovingOnX ? lastBlock.transform.localScale.x : lastBlock.transform.localScale.z;
        float newSize = currentSize - Mathf.Abs(hangover);

        // Se cortou demais (tamanho negativo), o jogo acaba
        if (newSize <= 0)
        {
            GameOver();
            return;
        }

        // Ajusta o tamanho da nova base e a posição para não sair do lugar
        float edge = isMovingOnX
            ? currentBlock.transform.position.x + (hangover > 0 ? -newSize / 2f : newSize / 2f)
            : currentBlock.transform.position.z + (hangover > 0 ? -newSize / 2f : newSize / 2f);

        Vector3 newScale = currentBlock.transform.localScale;
        Vector3 newPosition = currentBlock.transform.position;

        if (isMovingOnX)
        {
            newScale.x = newSize;
            newPosition.x = edge + (hangover > 0 ? newSize / 2f : -newSize / 2f);
        }
        else
        {
            newScale.z = newSize;
            newPosition.z = edge + (hangover > 0 ? newSize / 2f : -newSize / 2f);
        }

        currentBlock.transform.localScale = newScale;
        currentBlock.transform.position = newPosition;

        // Cria o pedaço que cai (Overhang)
        CreateFallingPiece(hangover, newSize);

        scoreManager.AddPoint();
        NextTurn();
    }

    private void CreateFallingPiece(float hangover, float newSize)
    {
        GameObject fallingPiece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Copia o material do bloco original (se houver)
        fallingPiece.GetComponent<Renderer>().material = currentBlock.GetComponent<Renderer>().material;

        Vector3 fallingScale = currentBlock.transform.localScale;
        Vector3 fallingPos = currentBlock.transform.position;

        if (isMovingOnX)
        {
            fallingScale.x = Mathf.Abs(hangover);
            fallingPos.x = currentBlock.transform.position.x + (hangover > 0 ? currentBlock.transform.localScale.x / 2f + fallingScale.x / 2f : -(currentBlock.transform.localScale.x / 2f + fallingScale.x / 2f));
        }
        else
        {
            fallingScale.z = Mathf.Abs(hangover);
            fallingPos.z = currentBlock.transform.position.z + (hangover > 0 ? currentBlock.transform.localScale.z / 2f + fallingScale.z / 2f : -(currentBlock.transform.localScale.z / 2f + fallingScale.z / 2f));
        }

        fallingPiece.transform.localScale = fallingScale;
        fallingPiece.transform.position = fallingPos;

        // Adiciona física para o pedaço cair
        fallingPiece.AddComponent<Rigidbody>();
        Destroy(fallingPiece, 3f); // Destrói após 3 segundos para limpar a memória
    }

    private void NextTurn()
    {
        lastBlock = currentBlock;
        isMovingOnX = !isMovingOnX; // Alterna o eixo
        stackCount++;
        SpawnNewBlock();
    }

    private void GameOver()
    {
        currentBlock.AddComponent<Rigidbody>(); // Faz o bloco atual cair inteiro
        Debug.Log("Game Over! Pontuação final: " + stackCount);
        // Aqui você pode chamar uma tela de Restart
    }
}