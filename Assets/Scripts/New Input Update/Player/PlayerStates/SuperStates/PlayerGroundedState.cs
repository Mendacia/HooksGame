using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected Vector2 input;
    private string lastState;

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

        if (input != Vector2.zero)
        {
            Vector2 signedInput = new Vector2(Mathf.Sign(input.x), Mathf.Sign(input.y));
            switch (signedInput)
            {
                case Vector2 v when v.Equals(new Vector2(1,1)):
                    {
                        //UP
                        if(lastState == "LEFT")
                        {
                            Debug.Log("Clockwise");
                        }
                        else if (lastState == "RIGHT")
                        {
                            Debug.Log("Anti-Clockwise");
                        }
                        lastState = "UP";
                        break;
                    }
                case Vector2 v when v.Equals(new Vector2(1,-1)):
                    {
                        //RIGHT
                        if (lastState == "UP")
                        {
                            Debug.Log("Clockwise");
                        }
                        else if (lastState == "DOWN")
                        {
                            Debug.Log("Anti-Clockwise");
                        }
                        lastState = "RIGHT";
                        break;
                    }
                case Vector2 v when v.Equals(new Vector2(-1, -1)):
                    {
                        //DOWN
                        if (lastState == "RIGHT")
                        {
                            Debug.Log("Clockwise");
                        }
                        else if (lastState == "LEFT")
                        {
                            Debug.Log("Anti-Clockwise");
                        }
                        lastState = "DOWN";
                        break;
                    }
                case Vector2 v when v.Equals(new Vector2(-1, 1)):
                    {
                        //LEFT
                        if (lastState == "DOWN")
                        {
                            Debug.Log("Clockwise");
                        }
                        else if (lastState == "UP")
                        {
                            Debug.Log("Anti-Clockwise");
                        }
                        lastState = "LEFT";
                        break;
                    }
            }
        }

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
