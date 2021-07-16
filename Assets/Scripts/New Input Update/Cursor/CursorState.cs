using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorState
{
    private string stateName;
    public bool isController = false;
    public CursorState(string stateName)
    {
        this.stateName = stateName;
    }

    public virtual void Enter(PlayerCursor playerCursor)
    {
        Debug.Log("Cursor State is now " + stateName);
    }
    public virtual void Exit(PlayerCursor playerCursor)
    {
    }

    public virtual void LogicUpdate(PlayerCursor playerCursor)//Update()
    {
    }

    public virtual void PhysicsUpdate(PlayerCursor playerCursor)//FixedUpdate()
    {
    }
}
