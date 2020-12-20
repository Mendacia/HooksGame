using UnityEngine;

public class InputGod : MonoBehaviour
{
    public KeyCode left, right, jump, up, down;
    private PauseMenu pauseScript;
    private bool menuIsOpen = false;
    private DevControls cheatScript;

    private void Awake()
    {
        Cursor.visible = false;
        pauseScript = GameObject.Find("Pause Menu").GetComponent<PauseMenu>();
        cheatScript = gameObject.GetComponent<DevControls>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1 || Time.timeScale == 0.2f)
            {
                Time.timeScale = 0;
                pauseScript.OpenMenu();
                menuIsOpen = true;
            }
        }
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        menuIsOpen = false;
    }
}
