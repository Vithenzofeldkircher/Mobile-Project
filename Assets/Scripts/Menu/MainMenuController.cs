using UnityEngine;
using UnityEngine.SceneManagement;   
public class MainMenuController : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    //Essa função serve para sair do jogo, apenas quando necessario.
    public void ExitGame()
    {
        Application.Quit();
    }

}
