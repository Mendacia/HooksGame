using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorState
{
    private string stateName;
    private Transform rhetical;
    private 
    public CursorState(string stateName)
    {
        this.stateName = stateName;
    }

    public virtual void Enter(PlayerCursor playerCursor)
    {
    }

    public virtual void LogicUpdate(PlayerCursor playerCursor)//Update()
    {
    }

    public virtual void PhysicsUpdate(PlayerCursor playerCursor)//FixedUpdate()
    {
    }
}
