using UnityEngine;
using System.Collections;

public class WindScript : MonoBehaviour
{
    public static bool IsWindActive { get; private set; } = false;
    public static int WindDirectionMultiplier { get; private set; } = 1;

    private bool isCheckingForWind = false;
    private float windDuration = 20f;

    [Header("Configurações do Vento")]
    [Tooltip("Tempo que o jogo espera antes de começar a sortear o primeiro vento (ex: 60 segundos).")]
    public float initialDelay = 40f; // Tempo de espera no início do jogo

    [Tooltip("Tempo em segundos entre cada tentativa do Random.Range após o delay inicial.")]
    public float checkInterval = 100f;

    void Start()
    {
        // Começa a rotina de sorteios automáticos
        StartCoroutine(WindCheckRoutine());
    }

    IEnumerator WindCheckRoutine()
    {
        print("Jogo iniciado. Aguardando o tempo inicial para habilitar os ventos...");

        // Espera o tempo inicial antes de permitir qualquer sorteio de vento
        yield return new WaitForSeconds(initialDelay);

        print("Tempo inicial concluído. Começando a monitorar e sortear ventos.");

        while (true)
        {
            // Se o vento já estiver ativo, espera o vento acabar antes de voltar a sortear
            if (IsWindActive)
            {
                yield return null;
                continue;
            }

            // Espera o intervalo determinado entre cada tentativa
            yield return new WaitForSeconds(checkInterval);

            // Gera o número aleatório entre 1 e 100
            int randomNumber = Random.Range(1, 100);
            print("Tentativa de vento - Número sorteado: " + randomNumber);

            if (randomNumber < 40)
            {
                // Vento ativado!
                StartCoroutine(ActivateWindRoutine());

                // Pausa esta rotina de sorteio enquanto o vento estiver rolando
                yield return new WaitForSeconds(windDuration);
            }
        }
    }

    IEnumerator ActivateWindRoutine()
    {
        IsWindActive = true;

        // Escolhe uma direção aleatória para o vento (1 para direita, -1 para esquerda)
        WindDirectionMultiplier = Random.Range(0, 2) == 0 ? 1 : -1;

        print("Vento ativado! Duração: 40 segundos.");

        // Mantém o vento ativo por 40 segundos
        yield return new WaitForSeconds(windDuration);

        // Desativa o vento
        IsWindActive = false;

        print("Vento desativado. Voltando a sortear periodicamente.");
    }
}