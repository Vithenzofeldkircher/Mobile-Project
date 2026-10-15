using System;
using UnityEngine;

[Serializable]
public class SmoothColorCycleStrategy : IColorStrategy
{
    [Header("Configurações de Cores")]
    [SerializeField] private Color[] colorList = new Color[] { Color.blue, Color.purple, Color.darkBlue };
    [SerializeField] private float transitionSpeed = 1f;

    private int currentIndex = 0;
    private float timer = 0f;

    public void Initialize()
    {
        currentIndex = 0;
        timer = 0f;
    }

    public Color GetCurrentColor(float time)
    {
        if (colorList == null || colorList.Length == 0) return Color.white;

        timer += Time.deltaTime * transitionSpeed;

        if (timer >= 1f)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % colorList.Length;
        }

        int nextIndex = (currentIndex + 1) % colorList.Length;

        // Faz a interpolação suave (Lerp) entre a cor atual e a próxima
        return Color.Lerp(colorList[currentIndex], colorList[nextIndex], timer);
    }
}