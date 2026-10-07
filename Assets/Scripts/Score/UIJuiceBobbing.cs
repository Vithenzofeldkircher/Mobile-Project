using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIJuiceBobbing : MonoBehaviour
{
    [Header("Balanço de Posição (Flutuação)")]
    [SerializeField] private bool animarPosicao = true;
    [SerializeField] private float amplitudePosicao = 8f;   // Distância do movimento em pixels
    [SerializeField] private float velocidadePosicao = 2f;   // Frequência do movimento

    [Header("Balanço de Rotação (Tilt)")]
    [SerializeField] private bool animarRotacao = true;
    [SerializeField] private float amplitudeRotacao = 2f;   // Ângulo em graus
    [SerializeField] private float velocidadeRotacao = 1.5f;

    [Header("Respiração (Escala)")]
    [SerializeField] private bool animarEscala = false;
    [SerializeField] private float amplitudeEscala = 0.03f; // Variação de escala (ex: 3%)
    [SerializeField] private float velocidadeEscala = 1.8f;

    private RectTransform rectTransform;
    private Vector3 posicaoInicial;
    private Vector3 escalaInicial;
    private float offsetAleatorio;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        posicaoInicial = rectTransform.anchoredPosition;
        escalaInicial = rectTransform.localScale;

        // Offset aleatório para que diferentes objetos na tela não balancem em sincronia perfeita
        offsetAleatorio = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float tempo = Time.time + offsetAleatorio;

        // 1. Movimento Suave na Posição (Cria sensação de "hand-held" / flutuação)
        if (animarPosicao)
        {
            float offsetY = Mathf.Sin(tempo * velocidadePosicao) * amplitudePosicao;
            float offsetX = Mathf.Cos(tempo * velocidadePosicao * 0.6f) * (amplitudePosicao * 0.3f);
            rectTransform.anchoredPosition = posicaoInicial + new Vector3(offsetX, offsetY, 0f);
        }

        // 2. Leve inclinação (Tilt)
        if (animarRotacao)
        {
            float rotZ = Mathf.Sin(tempo * velocidadeRotacao) * amplitudeRotacao;
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
        }

        // 3. Efeito de Respiração (Escala)
        if (animarEscala)
        {
            float fator = 1f + (Mathf.Sin(tempo * velocidadeEscala) * amplitudeEscala);
            rectTransform.localScale = escalaInicial * fator;
        }
    }
}