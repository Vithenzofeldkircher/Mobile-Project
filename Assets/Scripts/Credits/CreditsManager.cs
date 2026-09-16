using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [Header("Configurações de Transição de Cena")]
    [Tooltip("Nome exato da cena para a qual o jogo deve retornar")]
    public string targetSceneName;

    [Tooltip("Duração total dos créditos em segundos até trocar de cena")]
    [SerializeField] private float creditsDuration = 15f;

    [Header("Referências de UI")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private CreditsScroller creditsScroller;

    [Header("Aviso de Pular")]
    [SerializeField] private TextMeshProUGUI skipPromptText;
    [Tooltip("Tempo em segundos que o aviso ficará na tela")]
    [SerializeField] private float promptDisplayTime = 3f;

    private bool isCreditsActive = false;
    private float promptTimer = 0f;
    private float creditsTimer = 0f;

    private void Update()
    {
        if (!isCreditsActive) return;


        // 2. Cronômetro geral da duração dos créditos
        creditsTimer += Time.deltaTime;
        if (creditsTimer >= creditsDuration)
        {
            ReturnToMainScene();
            return;
        }

        // 3. Controle do tempo em que a mensagem de "Pular" fica visível
        if (skipPromptText != null && skipPromptText.gameObject.activeSelf)
        {
            promptTimer += Time.deltaTime;
            if (promptTimer >= promptDisplayTime)
            {
                skipPromptText.gameObject.SetActive(false);
            }
        }
    }

    // Chamado para iniciar a execução dos créditos
    public void OpenCredits()
    {
        isCreditsActive = true;
        creditsPanel.SetActive(true);

        promptTimer = 0f;
        creditsTimer = 0f;

        if (skipPromptText != null)
        {
            skipPromptText.gameObject.SetActive(true);
        }

        if (creditsScroller != null)
        {
            creditsScroller.BeginScroll();
        }
    }

    // Carrega a cena cujo nome foi inserido no Inspector
    public void ReturnToMainScene()
    {
        if (creditsScroller != null)
        {
            creditsScroller.StopScroll();
        }

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("O campo 'Target Scene Name' está vazio no Inspector do CreditsManager!");
        }
    }
}