using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenusScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        SceneManager.LoadSceneAsync("Settings");
    }
    
}
