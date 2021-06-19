using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(string animBoolName) : base(animBoolName)
    {
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        if(input.x != 0f)
        {
            player.ChangeState(new PlayerMoveState("move"));
        }
    }

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);
        player.StopThePlayer();
    }
}
