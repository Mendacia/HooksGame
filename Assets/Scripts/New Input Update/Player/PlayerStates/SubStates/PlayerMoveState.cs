using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(input.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            player.CheckIfShouldFlip((int)Mathf.Sign(input.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetAccelerationX(new Vector2 (input.x * playerData.movementAcceleration, 0), playerData.movementSpeedCap * Mathf.Abs(input.x));
    }
}
