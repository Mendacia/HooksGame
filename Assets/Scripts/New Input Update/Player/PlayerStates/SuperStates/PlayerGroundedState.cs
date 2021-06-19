using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected Vector2 input;
    private bool jumpInput;
    private bool isGrounded;
    public PlayerGroundedState(string animBoolName) : base(animBoolName)
    {
    }

    public override void DoChecks(Player player)
    {
        base.DoChecks(player);
        isGrounded = player.CheckIfGrounded();
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        input = player.InputHandler.MoveInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput)
        {
            player.InputHandler.UseJumpInput();
            player.ChangeState(new PlayerJumpState("inAir"));
        }
        else if (!isGrounded)
        {
            player.ChangeState(new PlayerInAirState("inAir", true));
        }
    }
}
