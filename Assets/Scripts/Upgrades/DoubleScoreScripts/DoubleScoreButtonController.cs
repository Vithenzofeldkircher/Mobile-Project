using UnityEngine;

public class DoubleScoreButtonController : MonoBehaviour
{
    [Header("Mensagem de Feedback")]
    [SerializeField] private string activationMessage = "Double Score Ativado! (2x Pontos)";

    private void Start()
    {
        if (!DoubleScoreManager.IsUnlocked() || DoubleScoreManager.IsActive)
        {
            Destroy(gameObject);
        }
    }

    public void ActivateDoubleScore()
    {
        if (DoubleScoreManager.IsUnlocked() && !DoubleScoreManager.IsActive)
        {
            DoubleScoreManager.IsActive = true;

            // Dispara a mensagem com Fade na tela
            if (NotificationFeedback.Instance != null)
            {
                NotificationFeedback.Instance.ShowMessage(activationMessage);
            }

            Debug.Log("<color=green>Double Score ativado com sucesso!</color>");
            Destroy(gameObject);
        }
    }
}