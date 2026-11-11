using UnityEngine;
using UnityEngine.SceneManagement;   
public class MainMenuController : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private GameObject MainShopMenu;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Title");
    }

    //Essa função serve para abrir a loja, apenas quando necessario.
    public void LoadShop()
    {
        MainShopMenu.SetActive(true);
    }

    // E essa função serve para fechar a loja, apenas quando necessario.
    public void CloseShop()
    {
        MainShopMenu.SetActive(false);
    }

    //Essa função serve para sair do jogo, apenas quando necessario.
    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
