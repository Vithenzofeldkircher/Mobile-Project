using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Acompanhamento")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 2f;
    [SerializeField] private float yOffset = 2f;

    [Header("Zoom Out Dinâmico")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float maxZoomOut = 12f;
    [SerializeField] private float alturaParaIniciarZoom = 5f;

    private Camera cam;
    private float initialOrthographicSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            initialOrthographicSize = cam.orthographicSize;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Acompanhamento suave no eixo Y
        Vector3 targetPosition = new Vector3(transform.position.x, target.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 2. Afastamento (Zoom Out) dinâmico baseado na altura da torre
        if (cam != null && cam.orthographicSize < maxZoomOut)
        {
            if (target.position.y > alturaParaIniciarZoom)
            {
                float targetZoom = initialOrthographicSize + ((target.position.y - alturaParaIniciarZoom) * 0.15f);
                targetZoom = Mathf.Clamp(targetZoom, initialOrthographicSize, maxZoomOut);

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
            }
        }
    }
}