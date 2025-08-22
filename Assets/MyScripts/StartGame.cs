using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
  public void StartTheGame()
  {
    // Load the main game scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
