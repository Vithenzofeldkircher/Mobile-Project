using UnityEngine;
using UnityEngine.UI;

public class DoubleScoreButtonController : MonoBehaviour
{
    [Header("Mensagens de Feedback")]
    [SerializeField] private string successMessage = "Double Score Ativado! (2x Pontos)";
    [SerializeField] private string notUnlockedMessage = "Upgrade não desbloqueado na loja!";
    [SerializeField] private string alreadyActiveMessage = "Double Score já está ativo!";

    [Header("Referências Visuais")]
    [SerializeField] private Button buttonComponent;

    private void Start()
    {
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
            // O botão só fica interativo se foi desbloqueado na loja E ainda não foi ativado nesta partida
            bool canUse = DoubleScoreManager.IsUnlocked() && !DoubleScoreManager.IsActive;
            buttonComponent.interactable = canUse;
        }
    }

    // Método vinculado ao evento OnClick() do Botão na UI
    public void ActivateDoubleScore()
    {
        if (!DoubleScoreManager.IsUnlocked())
        {
            if (NotificationFeedback.Instance != null)
                NotificationFeedback.Instance.ShowMessage(notUnlockedMessage);
            return;
        }

        if (DoubleScoreManager.IsActive)
        {
            if (NotificationFeedback.Instance != null)
                NotificationFeedback.Instance.ShowMessage(alreadyActiveMessage);
            return;
        }

        DoubleScoreManager.IsActive = true;

        if (NotificationFeedback.Instance != null)
        {
            NotificationFeedback.Instance.ShowMessage(successMessage);
        }

        UpdateVisualState();
    }
}