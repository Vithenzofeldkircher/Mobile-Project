using UnityEngine;

public class BlockMovement2D : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float boundX = 3f; // Limite da tela onde ele inverte a direção

    private bool isMoving = true;
    private int direction = 1;

    public void Initialize(bool startFromLeft)
    {
        // Define se nasce na esquerda (-1) ou direita (1)
        direction = startFromLeft ? 1 : -1;
        float startX = startFromLeft ? -boundX : boundX;
        transform.position = new Vector3(startX, transform.position.y, 0);
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void Update()
    {
        if (!isMoving) return;

        // Multiplica a velocidade por 2 se o vento estiver ativo
        float currentSpeed = moveSpeed;
        int currentDirection = direction;

        if (WindScript.IsWindActive)
        {
            currentSpeed *= 2f; // Dobra a velocidade
            currentDirection = WindScript.WindDirectionMultiplier; // Usa a direção do vento
        }

        // Move o bloco no eixo X
        transform.Translate(Vector3.right * currentDirection * currentSpeed * Time.deltaTime);

        // Se o vento NÃO estiver ativo, faz o comportamento de bate e volta normal
        if (!WindScript.IsWindActive)
        {
            if (transform.position.x >= boundX) direction = -1;
            else if (transform.position.x <= -boundX) direction = 1;
        }
        else
        {
            // Se o vento estiver ativo e passar do limite, o prefab volta para o início da tela
            if (transform.position.x >= boundX && currentDirection == 1)
            {
                transform.position = new Vector3(-boundX, transform.position.y, 0);
            }
            else if (transform.position.x <= -boundX && currentDirection == -1)
            {
                transform.position = new Vector3(boundX, transform.position.y, 0);
            }
        }
    }
}