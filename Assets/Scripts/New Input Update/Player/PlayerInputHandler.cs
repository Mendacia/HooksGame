using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;

    void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void onMoveInput(Vector2 myMoveInput) => MoveInput = myMoveInput;

    public void onJumpInput()
    {
        JumpInput = true;
        jumpInputStartTime = Time.time;
    }
    public void UseJumpInput() => JumpInput = false;
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
}
