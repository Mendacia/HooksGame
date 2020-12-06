using System.Collections;
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
    [System.NonSerialized] public bool playerIsCurrentlyInSomeKindOfHookState = false;
    [SerializeField]private bool canHookFromThisState = true;

    [Header("Set these to their respective scripts")]
    [SerializeField] private InputGod inputScript;
    private PlayerHookController hookController;
    private PlayerAnimatorController animScript;

    [Header("Visible in inspector for tweaking purposes")]
    [SerializeField] private float playerGroundedForce = 20;
    [SerializeField] private float playerJumpForce = 30;
    [SerializeField] private float airborneSpeedCap = 5;
    [SerializeField] private float defaultGravityScale = 5;

    [SerializeField] private float myMagnitude;




    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        hookController = GetComponent<PlayerHookController>();
        animScript = GetComponent<PlayerAnimatorController>();
        Time.timeScale = 1;
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
            default:
                {
                    break;
                }
        }
    }

    public void LeaveHookThrough()
    {
        //Note that SWING leaves through here too
        currentState = PlayerState.AIRBORNE;
    }
    public void LeaveHookDrop()
    {
        currentState = PlayerState.AIRBORNE;
        myRigidBody.velocity = Vector2.zero;
    }

    public void SetGrounded(bool grounded)
    {
        if (grounded && currentState != (PlayerState.DYING))
        {
            currentState = PlayerState.GROUNDED;
        }

        if (grounded == false)
        {
            if (currentState == PlayerState.GROUNDED)
            {
                currentState = PlayerState.AIRBORNE;
            }
        }
    }

    public void CanPlayerHook(bool canHook)
    {
        canHookFromThisState = canHook;
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
        }
        if (Input.GetMouseButtonDown(2) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.InitiateHook();
            hookController.playerIsHookingThrough = false;
        }
        if (Input.GetMouseButtonDown(1) && canHookFromThisState)
        {
            currentState = PlayerState.SWING;
            hookController.InitiateHook();
            hookController.SwingSetup();
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
    }

    private void AirborneControlsUpdate()
    {
        myRigidBody.gravityScale = defaultGravityScale; //Player needs to be weightless during HOOK. Resetting it.

        if (Input.GetKey(inputScript.right) && myRigidBody.velocity.x < airborneSpeedCap)
        {
            xIntent += 0.5f;
        }
        if (Input.GetKey(inputScript.left) && myRigidBody.velocity.x > -airborneSpeedCap)
        {
            xIntent -= 0.5f;
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
        }
        if (Input.GetMouseButtonDown(2) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.InitiateHook();
            hookController.playerIsHookingThrough = false;
        }
        if (Input.GetMouseButtonDown(1) && canHookFromThisState)
        {
            currentState = PlayerState.SWING;
            hookController.InitiateHook();
            hookController.SwingSetup();
        }

        if (xIntent == 0)
        {
            animScript.PlayJumpingAnimation(true, true);
        }
        else if (xIntent > 0)
        {
            animScript.PlayJumpingAnimation(true, false);
        }
        else if (xIntent < 0)
        {
            animScript.PlayJumpingAnimation(false, false);
        }


        playerIsCurrentlyInSomeKindOfHookState = false;

    }

    private void HookControlsUpdate()
    {
        canHookFromThisState = false;
        playerIsCurrentlyInSomeKindOfHookState = true;
        myRigidBody.gravityScale = 0;
    }

    private void SwingControlsUpdate()
    {
        canHookFromThisState = false;
        playerIsCurrentlyInSomeKindOfHookState = true;
        myRigidBody.gravityScale = 0;
        if (Input.GetMouseButtonUp(1))
        {
            hookController.SwingKiller();
        }
    }

    private void DyingUpdate()
    {
        canHookFromThisState = false;
        playerIsCurrentlyInSomeKindOfHookState = true;
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
