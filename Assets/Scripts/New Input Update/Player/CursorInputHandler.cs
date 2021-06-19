using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInputHandler : MonoBehaviour
{
    public Vector2 cursorPos { get; private set; }
    public bool HookInput { get; private set; }
    public bool SwingInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    private float hookInputStartTime;
    private float swingInputStartTime;

    void Update()
    {
        CheckHookInputHoldTime();
        CheckSwingInputHoldTime();
    }
    public void OnMousePosChange(Vector2 mousePos)
    {
        cursorPos = mousePos;
    }

    public void onHookInput()
    {
        HookInput = true;
        hookInputStartTime = Time.time;
    }
    public void UseHookInput() => HookInput = false;


    public void onSwingInput()
    {
        SwingInput = true;
        swingInputStartTime = Time.time;
    }
    public void UseJumpInput() => SwingInput = false;



    private void CheckHookInputHoldTime()
    {
        if (Time.time >= hookInputStartTime + inputHoldTime)
        {
            HookInput = false;
        }
    }
    private void CheckSwingInputHoldTime()
    {
        if (Time.time >= swingInputStartTime + inputHoldTime)
        {
            SwingInput = false;
        }
    }
}
