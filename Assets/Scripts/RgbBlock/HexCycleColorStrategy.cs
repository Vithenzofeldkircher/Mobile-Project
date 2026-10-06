using UnityEngine;

[System.Serializable]
public class HexCycleColorStrategy : IColorStrategy
{
    [Header("Configurações de Cor Hexadecimal")]
    [Tooltip("Lista de cores no formato Hexadecimal (ex: #FF5733, #33FF57, #3357FF)")]
    [SerializeField]
    private string[] hexColors = new string[]
    {
        "#FF5733",
        "#33FF57",
        "#3357FF",
        "#F333FF"
    };

    [Tooltip("Velocidade da transição de cores")]
    [SerializeField] private float transitionSpeed = 1f;

    private Color[] parsedColors;
    private bool isInitialized = false;


    public void Initialize()
    {
        if (hexColors == null || hexColors.Length == 0)
        {
            parsedColors = new Color[] { Color.white };
            isInitialized = true;
            return;
        }

        parsedColors = new Color[hexColors.Length];

        for (int i = 0; i < hexColors.Length; i++)
        {
            if (ColorUtility.TryParseHtmlString(hexColors[i], out Color parsedColor))
            {
                parsedColors[i] = parsedColor;
            }
            else
            {
                Debug.LogWarning($"[HexCycleColorStrategy] Código Hexadecimal inválido: '{hexColors[i]}'. Utilizando cor branca como padrão.");
                parsedColors[i] = Color.white;
            }
        }

        isInitialized = true;
    }

    public Color GetCurrentColor(float time)
    {
        if (!isInitialized) Initialize();
        if (parsedColors.Length == 1) return parsedColors[0];

        // Calcula a posição atual dentro do ciclo de cores
        float t = Mathf.Repeat(time * transitionSpeed, parsedColors.Length);
        int currentIndex = Mathf.FloorToInt(t);
        int nextIndex = (currentIndex + 1) % parsedColors.Length;

        float lerpFactor = t - currentIndex;

        // Suaviza a transição entre a cor atual e a próxima cor no array
        return Color.Lerp(parsedColors[currentIndex], parsedColors[nextIndex], lerpFactor);
    }
}