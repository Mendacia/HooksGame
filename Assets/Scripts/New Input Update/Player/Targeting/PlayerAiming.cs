using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    private enum controlType
    {
        MOUSE,
        RSTICK
    }
    private controlType currentControls = controlType.MOUSE;
    [SerializeField] private Camera currentCam;
    [SerializeField] private Transform rhetical;
    [SerializeField] private Transform player;
    [SerializeField] private float targetingRadius;

    //Variables for Mouse
    [SerializeField]private Vector2 cursorPos;

    //Variables for Gamepad


    //Called from input device change handler.
    public void ChangeControls(bool isGamepad)
    {
        if (isGamepad)
        {
            Debug.Log("Swapped to pad");
            currentControls = controlType.RSTICK;
        }
        else
        {
            Debug.Log("Swapped to mouse");
            currentControls = controlType.MOUSE;
        }
    }
    //Called via Event on GameManager
    public void OnMousePosChange(Vector2 mousePos)
    {
        cursorPos = mousePos;
    }


    void Update()
    {
        switch (currentControls)
        {
            case controlType.MOUSE:
                MouseModeUpdate();
                return;
            case controlType.RSTICK:
                GamepadModeUpdate();
                return;
            default:
                MouseModeUpdate();
                return;
        }
    }

    private void MouseModeUpdate()
    {
        rhetical.position = currentCam.ScreenToWorldPoint(new Vector3 (cursorPos.x, cursorPos.y, 100));
    }
    private void GamepadModeUpdate()
    {
        Vector2 pPos = new Vector2(player.position.x, player.position.y);
        Vector2 cPos = new Vector2(cursorPos.x, cursorPos.y);
        rhetical.position = Vector2.Lerp(rhetical.position, pPos + cPos * targetingRadius, 20f * Time.deltaTime);
    }
}
