using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool allowMovement;
    private bool isGrounded;
    private bool coyoteTime;
    public PlayerInAirState(string animBoolName, bool fromGrounded) : base(animBoolName)
    {
        if (fromGrounded) {
            StartCoyoteTime();
        }
    }

    public override void DoChecks(Player player)
    {
        base.DoChecks(player);
        isGrounded = player.CheckIfGrounded();
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        CheckCoyoteTime(player);
        if (isGrounded && player.CurrentVelocity.y < 0.01)
        {
            //This needs to be handled this way otherwise if you spam you can get multi jumps
            if(player.InputHandler.MoveInput.x == 0)
            {
                allowMovement = false;
                player.ChangeState(new PlayerLandState("land"));
            }
            else
            {
                allowMovement = false;
                player.ChangeState(new PlayerMoveState("move"));
            }
        }
        else if (player.InputHandler.JumpInput && coyoteTime)
        {
            coyoteTime = false;
            player.ChangeState(new PlayerJumpState("inAir"));
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

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);
        if (allowMovement)
        {
            player.SetAccelerationX(new Vector2(player.InputHandler.MoveInput.x * player.playerData.movementAcceleration, 0), player.playerData.movementSpeedCap * Mathf.Abs(player.InputHandler.MoveInput.x));
        }
    }
    private void CheckCoyoteTime(Player player)
    {
        if (coyoteTime && Time.time > startTime + player.playerData.coyoteTimeLength)
        {
            coyoteTime = false;
            Debug.Log("Coyote Time Ended");
        }
    }
    public void StartCoyoteTime() => coyoteTime = true;
}
