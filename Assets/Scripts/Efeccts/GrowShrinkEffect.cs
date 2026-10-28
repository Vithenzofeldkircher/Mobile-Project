using System.Collections.Generic; // Necessário para usar List
using UnityEngine;

public class GrowShrinkEffect : MonoBehaviour
{
    [Header("Scale Settings")]
    [Tooltip("Maximum scale multiplier (relative to original).")]
    public float maxScale = 1.5f;

    [Tooltip("Minimum scale multiplier (relative to original).")]
    public float minScale = 0.5f;

    [Tooltip("Speed of scaling.")]
    public float scaleSpeed = 2f;

    [Tooltip("Should the effect loop automatically?")]
    public bool loopEffect = true;

    [Header("Click Settings")]
    [Tooltip("Lista de GameObjects que serão desativados ao clicar na tela.")]
    public List<GameObject> objectsToDisable = new List<GameObject>();

    private Vector3 originalScale;
    private bool growing = true;

    void Start()
    {
        // Store the original scale to reset later
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Verifica se o jogador clicou com o botão esquerdo do mouse (0)
        if (Input.GetMouseButtonDown(0))
        {
            DisableObjects();
        }

        if (!loopEffect) return;

        // Determine target scale based on direction
        float targetScale = growing ? maxScale : minScale;

        // Smoothly interpolate towards target scale
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            originalScale * targetScale,
            Time.deltaTime * scaleSpeed
        );

        // Switch direction when close enough
        if (Vector3.Distance(transform.localScale, originalScale * targetScale) < 0.01f)
        {
            growing = !growing;
        }
    }

    public void TriggerGrowShrink()
    {
        growing = !growing;
    }

    // Função que percorre a lista e desativa os GameObjects
    public void DisableObjects()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}