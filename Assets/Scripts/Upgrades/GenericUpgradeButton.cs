using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GenericUpgradeButton : MonoBehaviour
{
    [Header("Configuração do Upgrade")]
    [SerializeField] private string upgradeID;
    [SerializeField] private int upgradeCost = 10; // Valor fixo do upgrade para cada compra

    [Header("Referências da UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private void Start()
    {
        UpdateUI();
    }

    public void BuyThisUpgrade()
    {
        // Tenta realizar a compra usando o custo fixo
        if (UpgradeManager.TryBuyUpgrade(upgradeID, upgradeCost))
        {
            Debug.Log($"Upgrade '{upgradeID}' comprado com sucesso!");

            // 1. Atualiza as informações do próprio botão (nível atual)
            UpdateUI();

            // 2. Localiza e atualiza imediatamente o texto de saldo (BestScore) na tela da loja
            ShowBestScore[] scoreDisplays = FindObjectsByType<ShowBestScore>(FindObjectsSortMode.None);
            foreach (var scoreDisplay in scoreDisplays)
            {
                scoreDisplay.RefreshBestScoreUI();
            }
        }
    }

    private void UpdateUI()
    {
        int currentLevel = UpgradeManager.GetUpgradeLevel(upgradeID);

        if (levelText != null)
            levelText.text = $"Nível: {currentLevel}";

        if (costText != null)
            costText.text = $"Custo: {upgradeCost}"; // Mostra sempre o valor fixo
    }
}