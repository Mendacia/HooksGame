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
    [SerializeField]private bool canHookFromThisState = true;

    [Header("Set these to their respective scripts")]
    [SerializeField] private InputGod inputScript;
    private PlayerHookController hookController;
    private CursorInputControls cursorControls;

    [Header("Visible in inspector for tweaking purposes")]
    [SerializeField] private float airborneSpeedCap = 5;
    [SerializeField] private float defaultGravityScale = 5;




    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        hookController = GetComponent<PlayerHookController>();
        Time.timeScale = 1;
    }

    private void Update()
    {
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
            default:
                {
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
            xIntent += 20;
        }
        if (Input.GetKey(inputScript.left))
        {
            xIntent -= 20;
        }
        if (Input.GetKey(inputScript.jump))
        {
                yIntent = 30;
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
            hookController.playerIsHookingThrough = true;
        }
        if (Input.GetMouseButtonDown(2) && canHookFromThisState)
        {
            currentState = PlayerState.HOOK;
            hookController.playerIsHookingThrough = false;
        }
    }

    private void HookControlsUpdate()
    {
        canHookFromThisState = false;
        myRigidBody.gravityScale = 0;
    }

    private void SwingControlsUpdate()
    {
        canHookFromThisState = false;
        myRigidBody.gravityScale = 0;
    }

    private void DyingUpdate()
    {
        canHookFromThisState = false;
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
