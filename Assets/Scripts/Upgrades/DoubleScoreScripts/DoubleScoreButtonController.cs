using UnityEngine;

public class DoubleScoreButtonController : MonoBehaviour
{
    private void Start()
    {
        // Se o jogador não comprou o upgrade na loja, o botão some da cena
        if (!DoubleScoreManager.IsUnlocked())
        {
            Destroy(gameObject);
            return;
        }

        // Se já estiver ativo nesta partida, também some
        if (DoubleScoreManager.IsActive)
        {
            Destroy(gameObject);
        }
    }

    // Método vinculado ao evento OnClick() do Botão na UI
    public void ActivateDoubleScore()
    {
        if (DoubleScoreManager.IsUnlocked() && !DoubleScoreManager.IsActive)
        {
            DoubleScoreManager.IsActive = true;
            Debug.Log("<color=green>Double Score ativado!</color>");

            // Destroi o botão/sprite imediatamente da cena
            Destroy(gameObject);
        }
    }
}