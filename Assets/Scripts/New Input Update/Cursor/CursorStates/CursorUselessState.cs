using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUselessState : CursorState
{
    public CursorUselessState(string stateName) : base(stateName)
    {
    }
    public override void Enter(PlayerCursor playerCursor)
    {
        base.Enter(playerCursor);
        playerCursor.SetRheticalVisibility(false);
    }
}
