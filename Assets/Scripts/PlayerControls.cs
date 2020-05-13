using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public KeyCode left, right, jump;
    public bool isGrounded;
    private Rigidbody2D rb;
    Vector2 wantedDirection;
    public HookThrough scriptThrough;

    private enum PlayerState
    {
        GROUNDED,
        AIRBORNE,
        HOOK,
        SWING,
    }

    private PlayerState currentState;

    private void Start()
    {
        //accessing the rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Setting up variables for moving the player later based on inputs
        var xIntent = rb.velocity.x;
        var yIntent = rb.velocity.y;

        if (currentState == PlayerState.GROUNDED)
        {
            xIntent = 0;
            //Taking inputs for LR player movement intent
            if (Input.GetKey(right))
            {
                xIntent += 5;
            }
            if (Input.GetKey(left))
            {
                xIntent -= 5;
            }

            //Taking inputs for the player's jump
            {
                if (Input.GetKey(jump))
                {
                    yIntent = 10;
                }
            }
        }
        //Actually moving the player
        wantedDirection = new Vector2(xIntent, yIntent);
        rb.velocity = wantedDirection;


        //Hook Controls
        if (currentState == PlayerState.GROUNDED || currentState == PlayerState.AIRBORNE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                scriptThrough.DestinationSetter();
                currentState = PlayerState.HOOK;
            }
        }
        if (currentState == PlayerState.HOOK)
        {
            scriptThrough.MoveThrough();
        }
    }


    //Leaving hook styles
    public void LeaveHookThrough()
    {
        currentState = PlayerState.AIRBORNE;
    }

    public void LeaveHookDrop()
    {
        currentState = PlayerState.AIRBORNE;
        rb.velocity = new Vector2(0,-20);
    }


    //referenced in GroundCheck, basically referencing a trigger collider at the player's feet that tells the controller when the player is touching the ground
    public void SetGrounded(bool grounded)
    {
        if (grounded)
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
}
