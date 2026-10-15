using UnityEngine;

public interface IColorStrategy
{
    void Initialize();
    Color GetCurrentColor(float time);
}