using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private CreditsScroller creditsScroller;

    private bool isCreditsActive = false;

    private void Update()
    {
        // Se os créditos estiverem ativos e o jogador clicar com o mouse ou tocar na tela
        if (isCreditsActive)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                CloseCredits();
            }
        }
    }

    // Chamado pelo botão do Menu Principal para abrir os créditos
    public void OpenCredits()
    {
        isCreditsActive = true;
        creditsPanel.SetActive(true);
        creditsScroller.BeginScroll();
    }

    // Fecha o painel e reseta tudo
    public void CloseCredits()
    {
        isCreditsActive = false;
        creditsScroller.StopScroll();
        creditsPanel.SetActive(false);
    }
}