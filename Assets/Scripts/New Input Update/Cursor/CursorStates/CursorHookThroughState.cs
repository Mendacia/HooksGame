using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHookThroughState : CursorState
{
    private Vector2 targetPosition;
    public CursorHookThroughState(string stateName, Vector2 targetPosition) : base(stateName)
    {
        this.targetPosition = targetPosition;
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

        playerCursor.TargeterFollowTarget(targetPosition);

        if (playerCursor.CheckIfPlayerIsInHookThrough() == false)
        {
            playerCursor.ChangeState(new CursorTargetingState("targeting"));
        }
    }
}
