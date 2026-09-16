using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    //essa função deve enviar o jogador para a cena de jogo
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    //essa função deve sair do jogo
    public void QuitGame()
    {
        Application.Quit();
    }
}
