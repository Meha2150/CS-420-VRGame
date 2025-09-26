using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameContoller : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Scene 1-Level 1");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
