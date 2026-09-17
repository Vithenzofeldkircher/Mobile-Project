using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Configurações de Acompanhamento")]
    [SerializeField] private float smoothSpeed = 4f; // Velocidade de transição suave
    [SerializeField] private float offsetY = 2f;      // Distância do topo do bloco até o centro da câmera

    private Transform cameraTransform;
    private Transform targetBlock;

    void Awake()
    {
        // Pega o componente Transform da própria Main Camera
        cameraTransform = GetComponent<Transform>();
    }

    public void SetTarget(Transform newTarget)
    {
        targetBlock = newTarget;
    }

    void LateUpdate()
    {
        if (targetBlock == null) return;

        // Posição Y desejada para que o bloco fique visível na tela
        float targetY = targetBlock.position.y + offsetY;

        // A câmera só sobe se a altura do bloco ultrapassar a posição Y atual da câmera
        if (targetY > cameraTransform.position.y)
        {
            Vector3 targetPosition = new Vector3(cameraTransform.position.x, targetY, cameraTransform.position.z);

            // Move a câmera suavemente para a nova altura
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}