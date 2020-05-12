using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Playing Game");
        SceneManager.LoadScene("Game");
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void ReturnMainMenu()
    {
        Debug.Log("Retunring to Main Menu");
        SceneManager.LoadScene("Start Screen");
    }
}
