using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(string animBoolName) : base(animBoolName)
    {
    }

    public override void Enter(Player player)
    {
        base.Enter(player);

        player.SetVelocityY(player.playerData.jumpVelocity);
        isAbilityDone = true;
    }
}
