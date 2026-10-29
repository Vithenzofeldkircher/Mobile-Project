using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private string name;

    public AudioClip Clip => clip;
    public string Name => name;
}
