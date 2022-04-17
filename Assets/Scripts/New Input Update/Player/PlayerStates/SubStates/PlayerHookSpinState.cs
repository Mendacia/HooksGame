using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookSpinState : PlayerAbilityState
{
    protected Vector2 input;
    private Transform targetHook;
    private float radius;
    private bool clockwise;
    private float rotationalVelocity;
    private string lastState;
    private float directionalIntent = 0;
    private float fallOutThreshold;
    public PlayerHookSpinState(string animBoolName, Transform targetPosition, float radius) : base(animBoolName)
    {
        targetHook = targetPosition;
        this.radius = radius;
    }

    public override void Enter(Player player)
    {
        base.Enter(player);
        player.DrawRope(targetHook.position);
        player.inSpinOrPreSpin = true;

        #region Spin Direction
        if (Mathf.Abs(player.RB.velocity.x) > Mathf.Abs(player.RB.velocity.y))
        {
            float xSigned = Mathf.Sign(player.RB.velocity.x);
            if (player.transform.position.y > targetHook.position.y)
            {
                if(xSigned > 0)
                {
                    clockwise = true; //Above the Hook and moving most prominently right
                }
                else
                {
                    clockwise = false; //Above the Hook and moving most prominently left
                }
            }
            else
            {
                if (xSigned > 0)
                {
                    clockwise = false; //Below the Hook and moving most prominently right
                }
                else
                {
                    clockwise = true; //Below the Hook and moving most prominently left
                }
            }
        }
        else
        {
            float ySigned = Mathf.Sign(player.RB.velocity.y);
            if(player.transform.position.x > targetHook.position.x)
            {
                if(ySigned > 0)
                {
                    clockwise = false; //Right of the hook and moving most prominently up
                }
                else
                {
                    clockwise = true; //Right of the hook and moving most prominently down
                }
            }
            else
            {
                if (ySigned > 0)
                {
                    clockwise = true; //Left of the hook and moving most prominently up
                }
                else
                {
                    clockwise = false; //Left of the hook and moving most prominently down
                }
            }
        }
        #endregion
        rotationalVelocity = (((player.RB.velocity.magnitude / radius) / (Mathf.PI / 30)) * 6 * (clockwise ? -1 : 1));
        player.SpinInitialVelocity(rotationalVelocity);
        fallOutThreshold = Mathf.Abs(rotationalVelocity);
        fallOutThreshold -= 0.5f;
    }

    public override void LogicUpdate(Player player)
    {
        input = player.InputHandler.MoveInput;
        base.LogicUpdate(player);
        player.UpdateRope(targetHook.position);

        if (input != Vector2.zero)
        {
            Vector2 signedInput = new Vector2(Mathf.Sign(input.x), Mathf.Sign(input.y));
            switch (signedInput)
            {
                case Vector2 v when v.Equals(new Vector2(1, 1)):
                    {
                        if (lastState == "LEFT")
                        {
                            directionalIntent = -1;
                        }
                        else if (lastState == "RIGHT")
                        {
                            directionalIntent = 1;
                        }
                        lastState = "UP";
                        break;
                    }
                case Vector2 v when v.Equals(new Vector2(1, -1)):
                    {
                        if (lastState == "UP")
                        {
                            directionalIntent = -1;
                        }
                        else if (lastState == "DOWN")
                        {
                            directionalIntent = 1;
                        }
                        lastState = "RIGHT";
                        break;
                    }
                case Vector2 v when v.Equals(new Vector2(-1, -1)):
                    {
                        if (lastState == "RIGHT")
                        {
                            directionalIntent = -1;
                        }
                        else if (lastState == "LEFT")
                        {
                            directionalIntent = 1;
                        }
                        lastState = "DOWN";
                        break;
                    }
                case Vector2 v when v.Equals(new Vector2(-1, 1)):
                    {
                        if (lastState == "DOWN")
                        {
                            directionalIntent = -1;
                        }
                        else if (lastState == "UP")
                        {
                            directionalIntent = 1;
                        }
                        lastState = "LEFT";
                        break;
                    }
            }
        }
        else
        {
            directionalIntent = 0;
        }

        if (Mathf.Abs(player.spinSpeed) < Mathf.Abs(fallOutThreshold))
        {
            player.SwingExit();
            player.AttemptSwingEntry(targetHook, radius, true);
        }
    }

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);
        player.OrbitalAcceleration(directionalIntent);
    }

    public override void Exit(Player player)
    {
        base.Exit(player);
        player.DestroyRope();
        player.inSpinOrPreSpin = false;
    }
}
