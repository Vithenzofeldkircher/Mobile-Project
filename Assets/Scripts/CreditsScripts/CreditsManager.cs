using UnityEngine;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private CreditsScroller creditsScroller;

    [Header("Aviso de Pular")]
    [SerializeField] private TextMeshProUGUI skipPromptText;
    [Tooltip("Tempo em segundos que o aviso ficará na tela")]
    [SerializeField] private float promptDisplayTime = 1f; // Tempo para o texto sumir

    private bool isCreditsActive = false;
    private float promptTimer = 0f; // Cronômetro interno

    private void Update()
    {
        if (isCreditsActive)
        {
            // 1. Verifica se o jogador apertou Space para pular
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CloseCredits();
                return; // Encerra o Update aqui para não rodar o resto do código à toa
            }

            // 2. Controla o tempo de exibição do texto (se ele ainda estiver ativo)
            if (skipPromptText.gameObject.activeSelf)
            {
                promptTimer += Time.deltaTime; // Conta o tempo

                // Se o tempo passado for maior ou igual ao tempo limite, desativa o texto
                if (promptTimer >= promptDisplayTime)
                {
                    skipPromptText.gameObject.SetActive(false);
                }
            }
        }
    }

    // Chamado pelo botão do Menu Principal para abrir os créditos
    public void OpenCredits()
    {
        isCreditsActive = true;
        creditsPanel.SetActive(true);

        // Ativa o aviso e zera o cronômetro sempre que abrir o painel
        skipPromptText.gameObject.SetActive(true);
        promptTimer = 0f;

        creditsScroller.BeginScroll();
    }

    // Fecha o painel e reseta tudo
    private void CloseCredits()
    {
        isCreditsActive = false;
        creditsScroller.StopScroll();
        creditsPanel.SetActive(false);
        skipPromptText.gameObject.SetActive(false);
    }
}