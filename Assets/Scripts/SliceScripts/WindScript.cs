using UnityEngine;
using System.Collections;

public class WindScript : MonoBehaviour
{
    public static bool IsWindActive { get; private set; } = false;
    public static int WindDirectionMultiplier { get; private set; } = 1;

    private float windDuration = 40f;

    [Header("Configurações do Vento")]
    [Tooltip("Tempo que o jogo espera antes de começar a sortear o primeiro vento.")]
    public float initialDelay = 30f;

    [Tooltip("Tempo em segundos entre cada tentativa do Random.Range após o delay inicial.")]
    public float checkInterval = 5f;

    private void Awake()
    {
        // Garante que o vento sempre comece desligado ao reiniciar a cena
        IsWindActive = false;
        WindDirectionMultiplier = 1;
    }

    void Start()
    {
        StartCoroutine(WindCheckRoutine());
    }

    IEnumerator WindCheckRoutine()
    {
        // Aguarda o tempo inicial antes de permitir qualquer sorteio de vento
        print("Aguardando o delay inicial de " + initialDelay + " segundos para os ventos...");
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // Espera o intervalo determinado entre cada tentativa
            yield return new WaitForSeconds(checkInterval);

            // Se o vento já estiver ativo por algum motivo, pula esta verificação
            if (IsWindActive) continue;

            // Gera o número aleatório entre 1 e 100
            int randomNumber = Random.Range(1, 100);
            print("Tentativa de vento - Número sorteado: " + randomNumber);

            if (randomNumber < 40)
            {
                // Vento ativado! Inicia a corrotina do vento
                StartCoroutine(ActivateWindRoutine());
            }
        }
    }

    IEnumerator ActivateWindRoutine()
    {
        IsWindActive = true;

        // Escolhe uma direção aleatória para o vento (1 para direita, -1 para esquerda)
        WindDirectionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;

        print("Vento ativado! Duração: 40 segundos. Direção: " + WindDirectionMultiplier);

        // Mantém o vento ativo por 40 segundos
        yield return new WaitForSeconds(windDuration);

        // Desativa o vento
        IsWindActive = false;

        print("Vento desativado. Retornando aos sorteios periódicos.");
    }
}