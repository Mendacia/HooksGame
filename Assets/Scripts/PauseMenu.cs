using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private bool isInLevelSelect = false;
    [SerializeField] private InputGod inputGodScript;
    public GameObject pauseUI;
    public Animator pauseAnims;
    private bool menuIsOpen = false;
    private bool menuIsOpenStalled = false;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& menuIsOpenStalled)
        {
            MenuContinueGame();
        }

        menuIsOpenStalled = menuIsOpen;
    }

    public void OpenMenu()
    {
        pauseAnims.Play("UI Pause Initialize");
        menuIsOpen = true;
        Cursor.visible = true;
    }

    public void MenuContinueGame()
    {
        pauseAnims.Play("UI Pause Close");
        menuIsOpen = false;
        Cursor.visible = false;
        inputGodScript.Resume();
    }

    public void returnToLevelSelect()
    {
        SceneManager.LoadScene("Template Scene");
    }

    public void QuitGame()
    {
        if (isInLevelSelect)
        {
            Application.Quit();
        }
        else
        {
            returnToLevelSelect();
        }
    }
}