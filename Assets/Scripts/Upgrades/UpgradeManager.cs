using UnityEngine;

public static class UpgradeManager
{
    private const string BEST_SCORE_KEY = "BestScore";

    // Retorna o nível atual de um upgrade específico baseado no ID dele
    public static int GetUpgradeLevel(string upgradeID)
    {
        return PlayerPrefs.GetInt($"Upgrade_{upgradeID}", 0);
    }

    // Tenta realizar a compra de qualquer upgrade usando o BestScore como moeda
    public static bool TryBuyUpgrade(string upgradeID, int cost)
    {
        int currentBestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);

        if (currentBestScore >= cost)
        {
            // Deduz o custo do recorde/moeda
            int newBestScore = currentBestScore - cost;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, newBestScore);
            PlayerPrefs.Save();

            // Incrementa o nível específico daquele upgrade
            int currentLevel = GetUpgradeLevel(upgradeID);
            PlayerPrefs.SetInt($"Upgrade_{upgradeID}", currentLevel + 1);
            PlayerPrefs.Save();

            return true;
        }

        Debug.LogWarning($"Pontuação insuficiente para comprar o upgrade: {upgradeID}!");
        return false;
    }
}