using UnityEngine;

public class FallBlocks : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    [SerializeField] private float maxTimeForSpawn = 2f; // Tempo máximo entre os spawns
    private float timeForSpawn;

    [Header("Arrays de Referência")]
    [SerializeField] private GameObject[] blocksPrefabs; // Lista de prefabs de blocos
    [SerializeField] private GameObject[] spawners;      // Pontos (GameObjects) onde os blocos vão nascer

    public bool PodeSpawnar { get; private set; } = true;

    private void Start()
    {
        timeForSpawn = maxTimeForSpawn;
    }

    private void Update()
    {
        if (!PodeSpawnar) return;

        // Diminui o tempo com base no tempo real do jogo
        timeForSpawn -= Time.deltaTime;

        // Quando o tempo zerar ou for menor que zero, faz o spawn
        if (timeForSpawn <= 0f)
        {
            SpawnBlock();
            // Reseta o cronômetro para o tempo máximo
            timeForSpawn = maxTimeForSpawn;
        }
    }

    public void SpawnBlock()
    {
        // Verifica se existem prefabs e spawners cadastrados para evitar erros
        if (blocksPrefabs.Length == 0 || spawners.Length == 0) return;

        // Escolhe um bloco aleatório do array
        int randomIndexBlock = Random.Range(0, blocksPrefabs.Length);

        // Escolhe um spawner (posição) aleatório do array
        int randomIndexSpawner = Random.Range(0, spawners.Length);

        // Pega a posição do spawner escolhido
        Vector3 spawnPosition = spawners[randomIndexSpawner].transform.position;
        Quaternion spawnRotation = spawners[randomIndexSpawner].transform.rotation;

        // Instancia o bloco escolhido na posição do spawner
        Instantiate(blocksPrefabs[randomIndexSpawner], spawnPosition, spawnRotation);

         Instantiate(blocksPrefabs[randomIndexBlock], spawnPosition, spawnRotation);
    }
}