using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    [Header("Configurações")]
    public float scrollSpeed = 50f; // Variável pública para controlar a velocidade
    [SerializeField] private float startYPosition = -1000f; // Posição Y inicial (abaixo da tela)

    [Header("Referências")]
    [SerializeField] private RectTransform textRectTransform; // Arraste o objeto de texto do TextMeshPro aqui

    private bool isScrolling = false;

    private void Update()
    {
        // Se estiver rolando, move a posição ancorada no eixo Y para cima, multiplicada pelo tempo
        if (isScrolling && textRectTransform != null)
        {
            textRectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

    // Inicia a rolagem a partir do zero
    public void BeginScroll()
    {
        ResetPosition();
        isScrolling = true;
    }

    // Para a rolagem
    public void StopScroll()
    {
        isScrolling = false;
    }

    // Reseta a posição do texto para a parte de baixo da tela
    private void ResetPosition()
    {
        if (textRectTransform != null)
        {
            Vector2 currentPos = textRectTransform.anchoredPosition;
            currentPos.y = startYPosition;
            textRectTransform.anchoredPosition = currentPos;
        }
    }
}