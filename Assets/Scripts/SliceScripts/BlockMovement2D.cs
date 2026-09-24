using UnityEngine;

public class BlockMovement2D : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float boundX = 3f;

    [Header("Dificuldade")]
    [SerializeField] private float velocidadeInicial = 3f;
    [SerializeField] private float incrementoVelocidade = 0.5f;
    [SerializeField] private int pontosParaAumentar = 5;

    private float moveSpeed;
    private bool isMoving = true;
    private int direction = 1;

    public void Initialize(bool startFromLeft, int currentScore)
    {
        direction = startFromLeft ? 1 : -1;
        float startX = startFromLeft ? -boundX : boundX;
        transform.position = new Vector3(startX, transform.position.y, 0);

        // Calcula a velocidade com base na pontuação (Responsabilidade do próprio bloco)
        CalcularVelocidade(currentScore);
    }

    private void CalcularVelocidade(int score)
    {
        moveSpeed = velocidadeInicial;

        // Laço de repetição para somar a velocidade a cada marco de pontos atingido
        for (int i = pontosParaAumentar; i <= score; i += pontosParaAumentar)
        {
            moveSpeed += incrementoVelocidade;
        }
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void Update()
    {
        if (!isMoving) return;

        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);

        if (transform.position.x >= boundX) direction = -1;
        else if (transform.position.x <= -boundX) direction = 1;
    }
}