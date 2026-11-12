using UnityEngine;

public class BlockCycleButtonController : MonoBehaviour
{
    [Header("Mensagens de Feedback")]
    [SerializeField] private string successMessage = "Bloco Trocado!";
    [SerializeField] private string noChargesMessage = "Sem cargas de Troca de Bloco!";

    private void Start()
    {
        if (!BlockCycleRunManager.HasCharges)
        {
            Destroy(gameObject);
        }
    }

    public void OnClickCycleBlock()
    {
        if (!BlockCycleRunManager.HasCharges)
        {
            if (NotificationFeedback.Instance != null)
                NotificationFeedback.Instance.ShowMessage(noChargesMessage);
            return;
        }

        StackManager2D stackManager = FindFirstObjectByType<StackManager2D>();
        if (stackManager != null)
        {
            stackManager.TryCycleBlock();
        }

        // Exibe a mensagem de sucesso com Fade
        if (NotificationFeedback.Instance != null)
        {
            NotificationFeedback.Instance.ShowMessage(successMessage);
        }

        if (!BlockCycleRunManager.HasCharges)
        {
            Destroy(gameObject);
        }
    }
}