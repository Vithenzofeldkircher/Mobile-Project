using UnityEngine;

public static class BlockCycleRunManager
{
    private const string UPGRADE_ID = "BlockCycle";
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