using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(string animBoolName) : base(animBoolName)
    {
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        if (input.x != 0)
        {
            player.ChangeState(new PlayerMoveState("move"));
        }
        else if(isAnimationFinished)
        {
            player.ChangeState(new PlayerIdleState("idle"));
        }
    }

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);
        player.StopThePlayer();
    }
}
