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

        // Move o bloco no eixo X
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);

        // Bateu no limite da tela, inverte a direção
        if (transform.position.x >= boundX) direction = -1;
        else if (transform.position.x <= -boundX) direction = 1;
    }
}