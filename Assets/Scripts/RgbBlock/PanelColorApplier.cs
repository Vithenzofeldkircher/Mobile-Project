using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PanelColorApplier : MonoBehaviour
{
    [Header("Estratégia Configurável")]
    [SerializeField] private SmoothColorCycleStrategy colorStrategy = new SmoothColorCycleStrategy();

    private Image uiImage;
    private IColorStrategy activeStrategy;

    void Awake()
    {
        // Obtém a referência do componente Image da UI
        uiImage = GetComponent<Image>();

        // Define a estratégia ativa respeitando o princípio de Inversão de Dependência (SOLID)
        activeStrategy = colorStrategy;
        activeStrategy.Initialize();
    }

    public void SetColorStrategy(IColorStrategy newStrategy)
    {
        activeStrategy = newStrategy;
        if (activeStrategy != null)
        {
            activeStrategy.Initialize();
        }
    }

    void Update()
    {
        if (activeStrategy != null && uiImage != null)
        {
            // Atualiza a cor do painel gradativamente com base na estratégia escolhida
            uiImage.color = activeStrategy.GetCurrentColor(Time.time);
        }
    }
}