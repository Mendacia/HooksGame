using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool allowMovement;
    private bool isGrounded;
    private bool coyoteTime;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
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
        CheckCoyoteTime();
        if (isGrounded && player.CurrentVelocity.y < 0.01)
        {
            //This needs to be handled this way otherwise if you spam you can get multi jumps
            if(player.InputHandler.MoveInput.x == 0)
            {
                allowMovement = false;
                stateMachine.ChangeState(player.LandState);
            }
            else
            {
                allowMovement = false;
                stateMachine.ChangeState(player.MoveState);
            }
        }
        else if (player.InputHandler.JumpInput && coyoteTime)
        {
            coyoteTime = false;
            stateMachine.ChangeState(player.JumpState);
        }
        else
        {
            if(player.InputHandler.MoveInput.x != 0)
            {
                player.CheckIfShouldFlip((int)Mathf.Sign(player.InputHandler.MoveInput.x));
            }
            allowMovement = true;

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (allowMovement)
        {
            player.SetAccelerationX(new Vector2(player.InputHandler.MoveInput.x * playerData.movementAcceleration, 0), playerData.movementSpeedCap * Mathf.Abs(player.InputHandler.MoveInput.x));
        }
    }
    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTimeLength)
        {
            coyoteTime = false;
            Debug.Log("Coyote Time Ended");
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
}
