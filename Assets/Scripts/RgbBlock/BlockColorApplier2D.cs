using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlockColorApplier2D : MonoBehaviour
{
    [Header("Estratégia Configurável")]
    [SerializeField] private HexCycleColorStrategy hexColorStrategy = new HexCycleColorStrategy();

    private SpriteRenderer spriteRenderer;
    private IColorStrategy activeStrategy;

    void Awake()
    {
        // Obtém a referência do SpriteRenderer no objeto
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Define a estratégia ativa
        activeStrategy = hexColorStrategy;
        hexColorStrategy.Initialize();
    }

    
    public void SetColorStrategy(IColorStrategy newStrategy)
    {
        activeStrategy = newStrategy;
    }

    void Update()
    {
        if (activeStrategy != null && spriteRenderer != null)
        {
            // Atualiza a tonalidade do SpriteRenderer gradativamente
            spriteRenderer.color = activeStrategy.GetCurrentColor(Time.time);
        }
    }
}