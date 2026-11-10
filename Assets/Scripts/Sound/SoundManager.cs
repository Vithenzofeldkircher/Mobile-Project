using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("Configurações da Playlist")]
    [SerializeField] private AudioClip[] playlist;
    [Range(0f, 1f)][SerializeField] private float targetVolume = 0.5f;
    [SerializeField] private float fadeDuration = 2.0f;

    private AudioSource audioSource;
    private int currentIndex = 0;

    private void Awake()
    {
        // Garante que o AudioSource está presente devido ao RequireComponent
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Desativado para gerenciar a transição manualmente
    }

    private void Start()
    {
        if (playlist != null && playlist.Length > 0)
        {
            PlayTrack(currentIndex);
        }
        else
        {
            Debug.LogWarning("A playlist está vazia no SoundManager.");
        }
    }

    private void Update()
    {
        // Verifica se a música acabou de tocar para avançar para a próxima
        if (!audioSource.isPlaying && playlist != null && playlist.Length > 0)
        {
            PlayNextTrack();
        }
    }

    private void PlayTrack(int index)
    {
        if (playlist[index] == null) return;

        audioSource.clip = playlist[index];
        audioSource.Play();

        // Inicia o efeito de Fade-In suave
        StopAllCoroutines();
        StartCoroutine(FadeInRoutine(targetVolume, fadeDuration));
    }

    private void PlayNextTrack()
    {
        // Incrementa o índice de forma circular usando o operador módulo (%)
        currentIndex = (currentIndex + 1) % playlist.Length;
        PlayTrack(currentIndex);
    }

    private IEnumerator FadeInRoutine(float maxVolume, float duration)
    {
        float currentTime = 0f;
        audioSource.volume = 0f; // Começa silencioso

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            // Aumenta o volume gradualmente até o limite definido (0.5 por padrão)
            audioSource.volume = Mathf.Lerp(0f, maxVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = maxVolume;
    }
}