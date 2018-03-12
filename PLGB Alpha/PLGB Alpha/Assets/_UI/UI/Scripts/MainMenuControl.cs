using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour {

    public Canvas canvasMainMenu;
    public ActivePlayersManager activePlayersManager;
    private void Start()
    {
        activePlayersManager = GetComponent<ActivePlayersManager>();
        activePlayersManager.RemoveAllPlayers();
    }

    public void OnPlay()
    {
        //next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }   
}
