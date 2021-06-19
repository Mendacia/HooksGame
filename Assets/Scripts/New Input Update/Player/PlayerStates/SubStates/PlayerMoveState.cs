using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(string animBoolName) : base(animBoolName)
    {
    }

    public override void LogicUpdate(Player player)
    {
        base.LogicUpdate(player);
        if(input.x == 0)
        {
            player.ChangeState(new PlayerIdleState("idle"));
        }
        else
        {
            player.CheckIfShouldFlip((int)Mathf.Sign(input.x));
        }
    }

    public override void PhysicsUpdate(Player player)
    {
        base.PhysicsUpdate(player);
        player.SetAccelerationX(new Vector2 (input.x * player.playerData.movementAcceleration, 0), player.playerData.movementSpeedCap * Mathf.Abs(input.x));
    }
}
