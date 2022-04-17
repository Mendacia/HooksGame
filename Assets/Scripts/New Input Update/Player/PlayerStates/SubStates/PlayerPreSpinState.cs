using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreSpinState : PlayerAbilityState
{
    private Transform targetHook;
    private float maximumRadius;
    private bool initialized = false;
    private bool enteringSpin = false;
    private bool overriden = false;
    float overrideThreshold = -1;
    public PlayerPreSpinState(string animBoolName, Transform targetPosition, float radius, bool ignore) : base(animBoolName)
    {
        this.targetHook = targetPosition;
        maximumRadius = radius;
        overriden = ignore;
    }


    public override void Enter(Player player)
    {
        base.Enter(player);
        player.inSpinOrPreSpin = true;
        if (overriden)
        {
            player.RB.velocity = new Vector2(player.RB.velocity.x * 0.5f, player.RB.velocity.y * 0.5f);
            overrideThreshold = 1.5f;
        }
        if (maximumRadius == 0)
        {
            maximumRadius = Vector2.Distance(player.transform.position, targetHook.position);
        }
    }


    public override void LogicUpdate(Player player)
    {
        if(overrideThreshold > 0)
        {
            overrideThreshold -= Time.deltaTime;
        }
        //Enter Swing
        if (Vector2.Distance(targetHook.position, (new Vector2(player.transform.position.x, player.transform.position.y) + (player.RB.velocity*Time.deltaTime))) > maximumRadius && initialized == false)
        {
            player.AttachRope(targetHook, maximumRadius);
            initialized = true;
        }

        if (initialized && player.RB.velocity.magnitude > ((2 * Mathf.PI * maximumRadius) / 1.7) && (Vector2.Angle(player.RB.velocity, (targetHook.position - player.transform.position)) < 135 && Vector2.Angle(player.RB.velocity, (targetHook.position - player.transform.position)) > 45) && overrideThreshold < 0)
        {
            player.SwingExit();
            player.SpinEntry(targetHook);
            player.ChangeState(new PlayerHookSpinState("spin", targetHook, maximumRadius));
        }

        if (player.InputHandler.MoveInput.x != 0)
        {
            player.CheckIfShouldFlip((int)Mathf.Sign(player.InputHandler.MoveInput.x));
        }

        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
    }

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);

        player.SetAccelerationX(new Vector2(player.InputHandler.MoveInput.x * player.playerData.movementAcceleration, 0), (2 * Mathf.PI * maximumRadius) * Mathf.Abs(player.InputHandler.MoveInput.x));
    }

    public override void Exit(Player player)
    {
        base.Exit(player);
        player.inSpinOrPreSpin = false;
    }
}
