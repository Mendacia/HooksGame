using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadedScene;

    public void PlayGame()
    {
        //Liam code bad. Ook.
        SceneManager.LoadScene(loadedScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Start Screen");
    }
}
