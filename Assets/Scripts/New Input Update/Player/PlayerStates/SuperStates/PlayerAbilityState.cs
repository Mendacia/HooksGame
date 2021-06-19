using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    private bool isGrounded;

    public PlayerAbilityState(string animBoolName) : base(animBoolName)
    {
    }

    public override void DoChecks(Player player)
    {
        base.DoChecks(player);
        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter(Player player)
    {
        base.Enter(player);
        isAbilityDone = false;
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        if (isAbilityDone)
        {
            if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                player.ChangeState(new PlayerIdleState("idle"));
            }
            else
            {
                player.ChangeState(new PlayerInAirState("inAir", false));
            }
        }
    }
}
