using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que encostou possui a tag "KillZone" (ou o nome que preferir para o seu limite inferior)
        // Ou simplesmente destrói ao tocar em qualquer trigger no fundo:
        if (collision.CompareTag("KillZone"))
        {
            Debug.Log("Colidiu com: " + collision.gameObject.name);
            // Destrói este GameObject (o bloco)
            Destroy(gameObject);
        }
        
        
    }
}