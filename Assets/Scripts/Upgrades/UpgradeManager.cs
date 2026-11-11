using UnityEngine;

public static class UpgradeManager
{
    private const string BEST_SCORE_KEY = "BestScore";

    // Retorna o nível de qualquer upgrade pelo seu ID único
    public static int GetUpgradeLevel(string upgradeID)
    {
        return PlayerPrefs.GetInt($"Upgrade_{upgradeID}", 0);
    }

    // Tenta realizar a compra debitando do BestScore e salvando o novo nível
    public static bool TryBuyUpgrade(string upgradeID, int cost)
    {
        int currentBestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);

        if (currentBestScore >= cost)
        {
            int newBestScore = currentBestScore - cost;
            PlayerPrefs.SetInt(BEST_SCORE_KEY, newBestScore);

            int currentLevel = GetUpgradeLevel(upgradeID);
            PlayerPrefs.SetInt($"Upgrade_{upgradeID}", currentLevel + 1);

            PlayerPrefs.Save(); // Garante a gravação imediata no disco
            return true;
        }

        Debug.LogWarning($"Moeda (BestScore) insuficiente para comprar: {upgradeID}");
        return false;
    }
}