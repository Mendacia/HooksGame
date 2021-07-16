using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTargetingState : CursorState
{
    private Transform targetHook = null;
    private int buffer = 0;
    private bool hookThroughInput = false;
    public CursorTargetingState(string stateName):base(stateName)
    {
    }
    public override void Enter(PlayerCursor playerCursor)
    {
        base.Enter(playerCursor);
        playerCursor.SetRheticalVisibility(true);
    }

    public override void LogicUpdate(PlayerCursor playerCursor)
    {
        base.LogicUpdate(playerCursor);
        if (playerCursor.usingController)
        {
            playerCursor.MoveCursorGamepad();
        }
        else
        {
            playerCursor.MoveCursorMouse();
        }

        targetHook = playerCursor.FindHookEligibility();
        if(targetHook == null && buffer <= 0)
        {
            playerCursor.TargeterFollowRhetical();
        }
        else if (targetHook != null)
        {
            buffer = 50;
            playerCursor.TargeterFollowTarget(targetHook.position);
        }
        else
        {
            playerCursor.TargeterReturnToRhetical();
            buffer--;
        }

        hookThroughInput = playerCursor.InputHandler.HookInput;
        if (hookThroughInput && targetHook != null)
        {
            playerCursor.InputHandler.UseHookInput();
            playerCursor.SetThePlayerStateToHookThroughState(targetHook.position);
            playerCursor.ChangeState(new CursorHookThroughState("hookThroughState", targetHook.position));
        }
    }
}
