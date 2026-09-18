using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    //essa função deve enviar o jogador para a cena de jogo
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Title");
    }

    //essa função deve sair do jogo
    public void QuitGame()
    {
        Application.Quit();
    }
}
