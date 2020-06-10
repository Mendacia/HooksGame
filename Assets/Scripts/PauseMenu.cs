using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public CursorControls cursorControlScript;
    public GameObject pauseUI;
    public Animator pauseAnims;

    public void OpenMenu()
    {
        pauseAnims.SetTrigger("OpenPause");
    }

    public void MenuContinueGame()
    {
        Debug.Log("Continue");
        pauseAnims.SetTrigger("ClosePause");
        cursorControlScript.ContinueGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}