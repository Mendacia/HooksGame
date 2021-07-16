using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookThroughState : PlayerAbilityState
{
    private Vector2 targetPosition;
    private float defaultGravityScale;
    public PlayerHookThroughState(string animBoolName, Vector2 targetPosition) : base(animBoolName)
    {
        this.targetPosition = targetPosition;
    }

    public override void Enter(Player player)
    {
        base.Enter(player);
        player.inHookThrough = true;
        defaultGravityScale = player.RB.gravityScale;
        player.RB.gravityScale = 0;
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        //DrawHook
    }

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);
        if (((targetPosition - player.RB.position).magnitude < (player.RB.velocity.magnitude * Time.deltaTime)) || targetPosition == new Vector2(player.transform.position.x, player.transform.position.y))
        {
            player.ChangeState(new PlayerInAirState("inAir", false));
        }
        else
        {
            player.SetVelocityHook(targetPosition, player.playerData.hookVelocity);
        }
    }

    public override void Exit(Player player)
    {
        base.Exit(player);
        //UndrawHook
        player.inHookThrough = false;
        player.RB.gravityScale = defaultGravityScale;
    }
}
