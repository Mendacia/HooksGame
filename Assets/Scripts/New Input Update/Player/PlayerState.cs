using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected bool isAnimationFinished;

    protected float startTime;

    private string animBoolName;

    public PlayerState(string animBoolName)
    {
        this.animBoolName = animBoolName;
    }

    public virtual void Enter(Player player)
    {
        DoChecks(player);
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        Debug.Log(animBoolName);
        isAnimationFinished = false;
    }

    public virtual void Exit(Player player)
    {
        player.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate(Player player)//Update()
    {

    }

    public virtual void PhysicsUpdate(Player player)//FixedUpdate()
    {
        DoChecks(player);
    }

    public virtual void DoChecks(Player player) { }

    public virtual void AnimationTrigger(Player player) { }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
