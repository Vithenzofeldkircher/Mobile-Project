using UnityEngine;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private CreditsScroller creditsScroller;

    private bool isCreditsActive = false;

    private void Update()
    {

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