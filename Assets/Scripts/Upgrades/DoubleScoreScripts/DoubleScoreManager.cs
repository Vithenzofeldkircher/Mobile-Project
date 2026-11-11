using UnityEngine;

public static class DoubleScoreManager
{
    private const string UPGRADE_ID = "DoubleScore";
    private static bool isActiveInCurrentRun = false;

    // Retorna se o jogador já comprou o upgrade na loja
    public static bool IsUnlocked()
    {
        return UpgradeManager.GetUpgradeLevel(UPGRADE_ID) > 0;
    }

    // Controla se o upgrade foi ativado pelo botão durante a partida atual
    public static bool IsActive
    {
        get => isActiveInCurrentRun;
        set => isActiveInCurrentRun = value;
    }

    // Reseta o estado para garantir que não venha ativado ao reiniciar ou trocar de cena
    public static void ResetRunState()
    {
        isActiveInCurrentRun = false;
    }
}