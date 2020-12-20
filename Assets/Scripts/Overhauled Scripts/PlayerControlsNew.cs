﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsNew : MonoBehaviour
{
    private enum PlayerState
    {
        GROUNDED,
        AIRBORNE,
        HOOK,
        SWING,
        DYING
    }
    private PlayerState currentState = PlayerState.AIRBORNE;

    private Rigidbody2D myRigidBody = null;
    private float xIntent;
    private float yIntent;
    private Vector2 wantedDirection;
    private Vector3 currentCheckpoint = Vector3.zero;
    [System.NonSerialized] public bool playerIsCurrentlyInSomeKindOfHookState = false;
    private bool canHookFromThisState = true;
    private bool isSwinging = false;
    private bool collidedWithTheFloorLastFrame = true;

    [Header("Set these to their respective things")]
    [SerializeField] private InputGod inputScript;
    [SerializeField] private GameObject deadClone = null;
    [SerializeField] private GameObject dustEffect = null;
    private List<DisableSemisolidWhileHooking> disableSemisolidScript = new List<DisableSemisolidWhileHooking>();
    private PlayerHookController hookController;
    private PlayerAnimatorController animScript;

    [Header("Visible in inspector for tweaking purposes")]
    [SerializeField] private float requiredMagnitudeForDustOnCollision = 50;
    [SerializeField] private float playerGroundedForce = 20;
    [SerializeField] private float playerAirborneMoveSpeed = 0.5f;
    [SerializeField] private float playerJumpForce = 30;
    [SerializeField] private float airborneSpeedCap = 5;
    [SerializeField] private float defaultGravityScale = 5;

    [SerializeField] private float myMagnitude = 0;




    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        hookController = GetComponent<PlayerHookController>();
        animScript = GetComponent<PlayerAnimatorController>();
        Time.timeScale = 1;
    }

    public void addMeToTheSemiSolidList(DisableSemisolidWhileHooking script)
    {
        disableSemisolidScript.Add(script);
        Debug.Log(disableSemisolidScript.Count);
    }

    private void Update()
    {
        myMagnitude = myRigidBody.velocity.magnitude;
        xIntent = myRigidBody.velocity.x;
        yIntent = myRigidBody.velocity.y;


        switch (currentState)
        {
            case PlayerState.GROUNDED:
                {
                    GroundedControlsUpdate();
                    break;
                }
            case PlayerState.AIRBORNE:
                {
                    AirborneControlsUpdate();
                    break;
                }
            case PlayerState.DYING:
                {
                    DyingUpdate();
                    break;
                }
            case PlayerState.SWING:
                {
                    SwingControlsUpdate();
                    break;
                }
            case PlayerState.HOOK:
                {
                    HookControlsUpdate();
                    break;
                }
        }

        //Intents are set in the switch statement's methods
        wantedDirection = new Vector2(xIntent, yIntent);
    }

    private void FixedUpdate()
    {
        myRigidBody.velocity = wantedDirection;
        //Running a switch again to run physics calculations in fixedUpdate
        switch (currentState)
        {
            case PlayerState.HOOK:
                {
                    HookControlsFixedUpdate();
                    break;
                }
            case PlayerState.SWING:
                {
                    SwingControlsFixedUpdate();
                    break;
                }
            case PlayerState.DYING:
            {
                break;
            }
            default:
                {
                    if (yIntent > 0.01)
                    {
                        foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
                        {
                            script.DisablePlatform();
                        }
                    }
                    else
                    {
                        foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
                        {
                            script.EnablePlatform();
                        }
                    }
                    break;
                }
        }
    }

    public void Die()
    {
        currentState = PlayerState.DYING;
    }

    public void LeaveHookThrough()
    {
        //Note that SWING leaves through here too
        currentState = PlayerState.AIRBORNE;
        hookController.killHookVisuals();
        foreach(DisableSemisolidWhileHooking script in disableSemisolidScript)
        {
            script.EnablePlatform();
        }
    }
    public void LeaveHookDrop()
    {
        currentState = PlayerState.AIRBORNE;
        hookController.killHookVisuals();
        myRigidBody.velocity = Vector2.zero;
        foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
        {
            script.EnablePlatform();
        }
    }

    public void SetCheckPointToThis(Vector3 here)
    {
        currentCheckpoint = here;
    }

    public void SetGrounded(bool grounded)
    {
        if (grounded && currentState != (PlayerState.DYING))
        {
            currentState = PlayerState.GROUNDED;
            if (isSwinging)
            {
                hookController.SwingKiller(true);
                isSwinging = false;
                hookController.killHookVisuals();
                foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
                {
                    script.EnablePlatform();
                }
            }
        }

        if (grounded == false)
        {
            if (currentState == PlayerState.GROUNDED)
            {
                currentState = PlayerState.AIRBORNE;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collidedWithTheFloorLastFrame = true;
        switch (currentState)
        {
            case PlayerState.HOOK:
                {
                    currentState = PlayerState.AIRBORNE;
                    hookController.killHookVisuals();
                    foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
                    {
                        script.EnablePlatform();
                    }
                    break;
                }
            case PlayerState.SWING:
                {
                    currentState = PlayerState.AIRBORNE;
                    hookController.SwingKiller(true);
                    isSwinging = false;
                    hookController.killHookVisuals();
                    foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
                    {
                        script.EnablePlatform();
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }

        if (collision.relativeVelocity.magnitude > requiredMagnitudeForDustOnCollision)
        {
            foreach (ContactPoint2D wallHit in collision.contacts)
            {
                Vector2 hitPoint = wallHit.point;
                Instantiate(dustEffect, new Vector3(hitPoint.x, hitPoint.y, 0), Quaternion.identity);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(currentState == PlayerState.SWING && collidedWithTheFloorLastFrame == false)
        {
            currentState = PlayerState.AIRBORNE;
            hookController.SwingKiller(true);
            isSwinging = false;
            hookController.killHookVisuals();
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.EnablePlatform();
            }
        }
    }

    private void LateUpdate()
    {
        if (currentState == PlayerState.SWING)
        {
            collidedWithTheFloorLastFrame = false;
        }
    }

    public void CanPlayerHook(bool canHook)
    {
        canHookFromThisState = canHook;
    }

    public bool ShouldILoadScene()
    {
        if (currentState == PlayerState.HOOK && hookController.playerIsHookingThrough == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetPlayerVelocity() => myRigidBody.velocity.magnitude;

    public bool GetPlayerCanHookFromState() => canHookFromThisState;


    //Everything after this is statemachine update functions


    private void GroundedControlsUpdate()
    {
        
        xIntent = 0; //Player Stops on a dime if they aren't making inputs.
        if (Input.GetKey(inputScript.right))
        {
            xIntent += playerGroundedForce;
        }
        if (Input.GetKey(inputScript.left))
        {
            xIntent -= playerGroundedForce;
        }
        if (Input.GetKey(inputScript.jump))
        {
                yIntent = playerJumpForce;
        }

        if (Input.GetMouseButtonDown(0) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.InitiateHook();
            hookController.playerIsHookingThrough = true;
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.DisablePlatform();
            }
        }
        if (Input.GetMouseButtonDown(2) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.InitiateHook();
            hookController.playerIsHookingThrough = false;
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.DisablePlatform();
            }
        }
        if (Input.GetMouseButtonDown(1) && canHookFromThisState)
        {
            currentState = PlayerState.SWING;
            hookController.InitiateHook();
            hookController.SwingSetup();
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.DisablePlatform();
            }
        }

        playerIsCurrentlyInSomeKindOfHookState = false;

        if(xIntent == 0 && yIntent == 0)
        {
            animScript.PlayIdleAnimation();
        }
        else if(xIntent > 0)
        {
            animScript.PlayWalkingAnimation(true);
        }
        else if (xIntent < 0)
        {
            animScript.PlayWalkingAnimation(false);
        }

        hookController.killHookVisuals();
    }

    private void AirborneControlsUpdate()
    {
        myRigidBody.gravityScale = defaultGravityScale; //Player needs to be weightless during HOOK. Resetting it.

        if (Input.GetKey(inputScript.right) && myRigidBody.velocity.x < airborneSpeedCap)
        {
            xIntent += playerAirborneMoveSpeed;
        }
        if (Input.GetKey(inputScript.left) && myRigidBody.velocity.x > -airborneSpeedCap)
        {
            xIntent -= playerAirborneMoveSpeed;
        }

        //Artificial Friction
        if (myRigidBody.velocity.x > airborneSpeedCap)
        {
            xIntent -= 1;
        }
        if (myRigidBody.velocity.x < -airborneSpeedCap)
        {
            xIntent += 1;
        }
        if (myRigidBody.velocity.y > airborneSpeedCap)
        {
            yIntent -= 0.5f;
        }

        if (Input.GetMouseButtonDown(0) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.InitiateHook();
            hookController.playerIsHookingThrough = true;
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.DisablePlatform();
            }
        }
        if (Input.GetMouseButtonDown(2) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.InitiateHook();
            hookController.playerIsHookingThrough = false;
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.DisablePlatform();
            }
        }
        if (Input.GetMouseButtonDown(1) && canHookFromThisState)
        {
            currentState = PlayerState.SWING;
            hookController.InitiateHook();
            hookController.SwingSetup();
            foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
            {
                script.DisablePlatform();
            }
        }

        bool isMovingRight =false;
        bool isntMoving = false;
        bool isMovingUp = true;
        if (xIntent == 0)
        {
            isMovingRight = true;
            isntMoving = true;
        }
        else if (xIntent > 0)
        {
            isMovingRight = true;
        }

        if(yIntent < 0)
        {
            isMovingUp = false;
        }
        animScript.PlayJumpingAnimation(isMovingRight, isntMoving, isMovingUp);


        playerIsCurrentlyInSomeKindOfHookState = false;

    }

    private void HookControlsUpdate()
    {
        canHookFromThisState = false;
        playerIsCurrentlyInSomeKindOfHookState = true;
        myRigidBody.gravityScale = 0;

        bool isMovingRight = false;
        bool isntMoving = false;
        bool isMovingUp = true;
        if (xIntent == 0)
        {
            isMovingRight = true;
            isntMoving = true;
        }
        else if (xIntent > 0)
        {
            isMovingRight = true;
        }

        if (yIntent < 0)
        {
            isMovingUp = false;
        }
        animScript.PlayJumpingAnimation(isMovingRight, isntMoving, isMovingUp);
    }

    private void SwingControlsUpdate()
    {
        canHookFromThisState = false;
        playerIsCurrentlyInSomeKindOfHookState = true;
        myRigidBody.gravityScale = 0;
        isSwinging = true;

        bool isMovingRight = false;
        bool isntMoving = false;
        bool isMovingUp = true;
        if (xIntent == 0)
        {
            isMovingRight = false;
            isntMoving = true;
        }
        else if (xIntent > 0)
        {
            isMovingRight = true;
        }

        if (yIntent < 0)
        {
            isMovingUp = false;
        }
        animScript.PlayJumpingAnimation(isMovingRight, isntMoving, isMovingUp);

        if (Input.GetMouseButtonUp(1))
        {
            hookController.SwingKiller(false);
            isSwinging = false;
        }


    }

    private void DyingUpdate()
    {
        if (isSwinging)
        {
            hookController.SwingKiller(true);
            isSwinging = false;
        }
        hookController.killHookVisuals();
        foreach (DisableSemisolidWhileHooking script in disableSemisolidScript)
        {
            script.EnablePlatform();
        }

        playerIsCurrentlyInSomeKindOfHookState = false;
        canHookFromThisState = false;
        Time.timeScale = Mathf.Lerp(Time.timeScale, 0, 10 * Time.unscaledDeltaTime);

        if (Input.anyKeyDown)
        {
            Instantiate(deadClone, gameObject.transform.position, gameObject.transform.rotation);
            myRigidBody.velocity = Vector3.zero;
            gameObject.transform.position = currentCheckpoint;
            Time.timeScale = 1;
            currentState = PlayerState.AIRBORNE;
        }
    }

    private void HookControlsFixedUpdate()
    {
        hookController.MoveThrough();
    }

    private void SwingControlsFixedUpdate()
    {
        hookController.Swing();
    }
}
