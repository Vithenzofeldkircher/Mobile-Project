using UnityEngine;
using System.Collections;

public class WindScript : MonoBehaviour
{
    public static bool IsWindActive { get; private set; } = false;
    public static int WindDirectionMultiplier { get; private set; } = 1;

    private float windDuration = 40f;
    private Coroutine activeWindCoroutine; // Referência para podermos parar a corrotina se necessário

    [Header("Configurações do Vento")]
    [Tooltip("Tempo que o jogo espera antes de começar a sortear o primeiro vento.")]
    public float initialDelay = 30f;

    [Tooltip("Tempo em segundos entre cada tentativa do Random.Range após o delay inicial.")]
    public float checkInterval = 5f;

    private void Awake()
    {
        IsWindActive = false;
        WindDirectionMultiplier = 1;
    }

    void Start()
    {
        StartCoroutine(WindCheckRoutine());
    }

    IEnumerator WindCheckRoutine()
    {
        print("Aguardando o delay inicial de " + initialDelay + " segundos para os ventos...");
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (IsWindActive) continue;

            int randomNumber = Random.Range(1, 100);
            print("Tentativa de vento - Número sorteado: " + randomNumber);

            if (randomNumber < 40)
            {
                activeWindCoroutine = StartCoroutine(ActivateWindRoutine());
            }
        }
    }

    IEnumerator ActivateWindRoutine()
    {
        IsWindActive = true;
        WindDirectionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;

        print("Vento ativado! Duração: 40 segundos. Direção: " + WindDirectionMultiplier);

        yield return new WaitForSeconds(windDuration);

        IsWindActive = false;
        print("Vento desativado. Retornando aos sorteios periódicos.");
    }

    // NOVO MÉTODO: Permite interromper o vento ativamente e aplicar o delay de respiro
    public void StopWindManually(float stopDelay)
    {
        // Se o vento não estiver ativo, usa o return para sair imediatamente
        if (!IsWindActive) return;

        // Para a corrotina atual do vento que estava rodando
        if (activeWindCoroutine != null)
        {
            StopCoroutine(activeWindCoroutine);
        }

        StartCoroutine(ForceStopWindRoutine(stopDelay));
    }

    IEnumerator ForceStopWindRoutine(float stopDelay)
    {
        IsWindActive = false;
        print($"Vento interrompido pelo jogador! Aguardando delay de {stopDelay} segundos para voltar...");

        // Aguarda o delay em float fornecido pelo botão
        yield return new WaitForSeconds(stopDelay);

        print("Delay encerrado. Os ventos já podem voltar a ser sorteados normalmente.");
        // O loop principal (WindCheckRoutine) continuará rodando e sorteando novos ventos.
    }
}