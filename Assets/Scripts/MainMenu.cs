using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string loadedScene;
    [SerializeField] private GameObject creditsPanel = null;

    private void Awake()
    {
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        //Liam code bad. Ook.
        SceneManager.LoadScene(loadedScene);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
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
