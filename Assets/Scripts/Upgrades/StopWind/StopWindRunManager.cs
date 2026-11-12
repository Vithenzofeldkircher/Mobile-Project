using UnityEngine;

public static class StopWindRunManager
{
    private const string UPGRADE_ID = "StopWind";
    private static int chargesLeft = 0;

    // Chamado no início da partida para carregar quantos usos o jogador comprou na loja
    public static void InitializeRun()
    {
        int purchasedLevel = UpgradeManager.GetUpgradeLevel(UPGRADE_ID);
        chargesLeft = purchasedLevel; // Cada nível comprado equivale a 1 uso por partida
    }

    public static bool HasCharges => chargesLeft > 0;

    public static int ChargesLeft => chargesLeft;

    public static void ConsumeCharge()
    {
        if (chargesLeft > 0)
        {
            chargesLeft--;
        }
    }

    public static void ResetRun()
    {
        chargesLeft = 0;
    }
}