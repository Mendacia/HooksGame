using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public CursorControls cursorControlScript;
    public GameObject pauseUI;
    public Animator pauseAnims;

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    public void OpenMenu()
    {
        pauseUI.SetActive(true);
        pauseAnims.SetTrigger("OpenPause");
    }

    public void MenuContinueGame()
    {
        pauseUI.SetActive(false);
        Debug.Log("Continue");
        cursorControlScript.ContinueGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}