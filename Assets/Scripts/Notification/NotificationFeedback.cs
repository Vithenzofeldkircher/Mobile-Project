using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationFeedback : MonoBehaviour
{
    public static NotificationFeedback Instance { get; private set; }

    [Header("Referências de UI")]
    [SerializeField] private TextMeshProUGUI notificationText;

    [Header("Configurações de Fade")]
    [SerializeField] private float fadeDuration = 0.4f;    // Tempo do Fade In / Fade Out
    [SerializeField] private float displayDuration = 1.5f; // Tempo que a mensagem fica visível na tela

    private Coroutine activeCoroutine;

    private void Awake()
    {
        // Padrão Singleton simples para acesso global fácil
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Garante que o texto comece totalmente invisível
        if (notificationText != null)
        {
            Color col = notificationText.color;
            col.a = 0f;
            notificationText.color = col;
        }
    }

    // Método público chamado por qualquer upgrade para exibir a mensagem
    public void ShowMessage(string message)
    {
        if (notificationText == null) return;

        // Se já houver uma mensagem rodando, para a anterior para evitar conflito de alpha
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(FadeRoutine(message));
    }

    private IEnumerator FadeRoutine(string message)
    {
        notificationText.text = message;
        Color color = notificationText.color;

        // 1. FADE IN (De 0 para 1 de Alpha)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            notificationText.color = color;
            yield return null;
        }
        color.a = 1f;
        notificationText.color = color;

        // 2. ESPERA (Mantém legível na tela)
        yield return new WaitForSeconds(displayDuration);

        // 3. FADE OUT (De 1 para 0 de Alpha)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            notificationText.color = color;
            yield return null;
        }
        color.a = 0f;
        notificationText.color = color;
    }
}