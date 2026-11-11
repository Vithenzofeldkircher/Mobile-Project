using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GenericUpgradeButton : MonoBehaviour
{
    [Header("Configuração do Upgrade")]
    [Tooltip("Identificador único deste upgrade (Ex: 'BlockType', 'Tolerance', 'ExtraLife')")]
    [SerializeField] private string upgradeID;
    [SerializeField] private int baseCost = 10;
    //[SerializeField] private float costMultiplier = 1.5f; // Opcional: aumenta o custo a cada nível

    [Header("Referências da UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private void Start()
    {
        UpdateUI();
    }

    // Método universal vinculado ao evento OnClick() do Botão na UI
    public void BuyThisUpgrade()
    {
        int currentCost = CalculateCurrentCost();

        if (UpgradeManager.TryBuyUpgrade(upgradeID, currentCost))
        {
            Debug.Log($"Upgrade '{upgradeID}' comprado com sucesso!");
            UpdateUI();
        }
    }

    private int CalculateCurrentCost()
    {
        int currentLevel = UpgradeManager.GetUpgradeLevel(upgradeID);
        // Exemplo: Cust aumenta proporcionalmente ao nível atual
        return baseCost;
    }

    private void UpdateUI()
    {
        int currentLevel = UpgradeManager.GetUpgradeLevel(upgradeID);
        int currentCost = CalculateCurrentCost();

        if (levelText != null)
            levelText.text = $"Nível: {currentLevel}";

        if (costText != null)
            costText.text = $"Custo: {currentCost}";
    }
}