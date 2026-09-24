using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlockColorChanger2D : MonoBehaviour
{
    [Header("Configuração de Cores")]
    [SerializeField] private float speed = 1f;

    // Lista de cores em Hexadecimal para alternar suavemente
    [SerializeField] private string[] hexColors = { "#FF5733", "#33FF57", "#3357FF", "#F3FF33", "#FF33F3" };

    private SpriteRenderer spriteRenderer;
    private Color[] colors;
    private int currentIndex = 0;
    private int nextIndex = 1;
    private float t = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ConvertHexToColors();
    }

    private void ConvertHexToColors()
    {
        if (hexColors == null || hexColors.Length == 0) return;

        colors = new Color[hexColors.Length];
        for (int i = 0; i < hexColors.Length; i++)
        {
            if (ColorUtility.TryParseHtmlString(hexColors[i], out Color parsedColor))
            {
                colors[i] = parsedColor;
            }
            else
            {
                colors[i] = Color.white;
            }
        }
    }

    void Update()
    {
        if (colors == null || colors.Length < 2) return;

        t += Time.deltaTime * speed;

        // Transição suave entre a cor atual e a próxima
        spriteRenderer.color = Color.Lerp(colors[currentIndex], colors[nextIndex], t);

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length;
        }
    }
}