using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Configurações de Acompanhamento")]
    [SerializeField] private float smoothSpeed = 4f;
    [SerializeField] private float offsetY = 2f;

    [Header("Configurações de Zoom no Game Over")]
    [SerializeField] private float baseOrthographicSize = 5f; // Tamanho normal da câmera no jogo
    [SerializeField] private float maxOrthographicSize = 25f;  // Limite máximo de afastamento

    private Camera cam;
    private Transform targetBlock;
    private bool isGameOverView = false;

    private float targetYPosition;
    private float targetCamSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = baseOrthographicSize;
        targetCamSize = baseOrthographicSize;
    }

    public void SetTarget(Transform newTarget)
    {
        targetBlock = newTarget;
    }

    void LateUpdate()
    {
        // COMPORTAMENTO NO GAME OVER: Afasta e centraliza a torre inteira
        if (isGameOverView)
        {
            // Transição suave do zoom
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetCamSize, smoothSpeed * Time.deltaTime);

            // Transição suave para o centro da torre
            Vector3 targetPos = new Vector3(transform.position.x, targetYPosition, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
            return;
        }

        // COMPORTAMENTO DURANTE A PARTIDA: Apenas acompanha a subida do bloco
        if (targetBlock == null) return;

        float targetY = targetBlock.position.y + offsetY;

        // A câmera só sobe quando o bloco ultrapassa sua altura atual
        if (targetY > transform.position.y)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }


    public void ZoomOutToOverview(float totalTowerHeight)
    {
        isGameOverView = true;

        // Posiciona a câmera no centro de altura total da torre
        targetYPosition = totalTowerHeight / 2f;

        // Calcula o afastamento necessário com margem de segurança, sem ultrapassar o limite máximo
        float requiredSize = (totalTowerHeight / 2f) + 3f;
        targetCamSize = Mathf.Clamp(requiredSize, baseOrthographicSize, maxOrthographicSize);
    }
}