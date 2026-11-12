using UnityEngine;
using UnityEngine.UI;

public class BlockCycleButtonController : MonoBehaviour
{
    [Header("Mensagens de Feedback")]
    [SerializeField] private string successMessage = "Bloco Trocado!";
    [SerializeField] private string noChargesMessage = "Sem cargas de Troca de Bloco!";

    [Header("Referências Visuais")]
    [SerializeField] private Button buttonComponent;

    private void Start()
    {
        BlockCycleRunManager.InitializeRun();
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
            buttonComponent.interactable = BlockCycleRunManager.HasCharges;
        }
    }

    // Método vinculado ao evento OnClick() do Botão na UI
    public void OnClickCycleBlock()
    {
        if (!BlockCycleRunManager.HasCharges)
        {
            if (NotificationFeedback.Instance != null)
            {
                NotificationFeedback.Instance.ShowMessage(noChargesMessage);
            }
            return;
        }

        StackManager2D stackManager = FindFirstObjectByType<StackManager2D>();
        if (stackManager != null)
        {
            stackManager.TryCycleBlock();
        }

        if (NotificationFeedback.Instance != null)
        {
            NotificationFeedback.Instance.ShowMessage(successMessage);
        }

        UpdateVisualState();
    }
}