using System;
using UnityEngine;

[Serializable]
public class ParticleEffectService : ITowerEffectService
{
    [SerializeField] private GameObject explosionParticlePrefab;

    public void PlayPlacementEffect(Vector3 position)
    {
        if (explosionParticlePrefab == null) return;

        // Instancia a partícula na posição do acerto
        GameObject particleInstance = UnityEngine.Object.Instantiate
        (
            explosionParticlePrefab,
            position,
            Quaternion.identity
        );

        // Tenta pegar a duração da partícula para destruir no tempo correto
        ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
        float destroyDelay = ps != null ? ps.main.duration + ps.main.startLifetime.constantMax : 2f;

        UnityEngine.Object.Destroy(particleInstance, destroyDelay);
    }
}