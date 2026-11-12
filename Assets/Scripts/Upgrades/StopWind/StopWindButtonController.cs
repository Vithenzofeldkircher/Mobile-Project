using UnityEngine;

public class StopWindButtonController : MonoBehaviour
{
    [Header("Configurações do Upgrade")]
    [SerializeField] private float stopDelay = 5f;

    [Header("Mensagens de Feedback")]
    [SerializeField] private string successMessage = "Ventos Parados!";
    [SerializeField] private string noChargesMessage = "Sem cargas de Parar Vento!";

    private void Start()
    {
        StopWindRunManager.InitializeRun();

        if (!StopWindRunManager.HasCharges)
        {
            Destroy(gameObject);
        }
    }

    public void OnClickStopWind()
    {
        if (!StopWindRunManager.HasCharges)
        {
            if (NotificationFeedback.Instance != null)
                NotificationFeedback.Instance.ShowMessage(noChargesMessage);
            return;
        }

        StopWindRunManager.ConsumeCharge();

        WindScript windScript = FindFirstObjectByType<WindScript>();
        if (windScript != null)
        {
            windScript.StopWindManually(stopDelay);
        }

        // Exibe a mensagem de sucesso com Fade
        if (NotificationFeedback.Instance != null)
        {
            NotificationFeedback.Instance.ShowMessage(successMessage);
        }

        Debug.Log($"<color=cyan>Botão de Parar Vento acionado!</color> Cargas restantes: {StopWindRunManager.ChargesLeft}");

        if (!StopWindRunManager.HasCharges)
        {
            Destroy(gameObject);
        }
    }
}