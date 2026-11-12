using UnityEngine;
using UnityEngine.UI;

public class StopWindButtonController : MonoBehaviour
{
    [Header("Configurações do Upgrade")]
    [SerializeField] private float stopDelay = 5f;

    [Header("Mensagens de Feedback")]
    [SerializeField] private string successMessage = "Ventos Parados!";
    [SerializeField] private string noChargesMessage = "Sem cargas de Parar Vento!";

    [Header("Referências Visuais")]
    [SerializeField] private Button buttonComponent;

    private void Start()
    {
        StopWindRunManager.InitializeRun();
        if (buttonComponent == null) buttonComponent = GetComponent<Button>();
        UpdateVisualState();
    }

    private void Update()
    {
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        if (buttonComponent != null)
        {
            // Fica interativo apenas se houver cargas disponíveis para esta partida
            buttonComponent.interactable = StopWindRunManager.HasCharges;
        }
    }

    // Método vinculado ao evento OnClick() do Botão na UI
    public void OnClickStopWind()
    {
        if (!StopWindRunManager.HasCharges)
        {
            if (NotificationFeedback.Instance != null)
            {
                NotificationFeedback.Instance.ShowMessage(noChargesMessage);
            }
            return;
        }

        StopWindRunManager.ConsumeCharge();

        WindScript windScript = FindFirstObjectByType<WindScript>();
        if (windScript != null)
        {
            windScript.StopWindManually(stopDelay);
        }

        if (NotificationFeedback.Instance != null)
        {
            NotificationFeedback.Instance.ShowMessage(successMessage);
        }

        UpdateVisualState();
    }
}