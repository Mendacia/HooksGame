﻿using System.Collections;
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

    //Bug Fixing Variables
    public float xVelocity;

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
        xVelocity = xIntent;

        if (currentState == PlayerState.GROUNDED)
        {
            xIntent = 0;
            //Taking inputs for LR player movement intent
            if (Input.GetKey(right))
            {
                xIntent += 20;
            }
            if (Input.GetKey(left))
            {
                xIntent -= 20;
            }

            //Taking inputs for the player's jump
            {
                if (Input.GetKey(jump))
                {
                    yIntent = 30;
                }
            }
        }

        if (currentState == PlayerState.AIRBORNE)
        {
            rb.gravityScale = 5;
            //Taking inputs for LR player movement intent
            if (Input.GetKey(right) && rb.velocity.x < 5)
            {
                xIntent += 0.3f;
            }
            if (Input.GetKey(left) && rb.velocity.x > -5)
            {
                xIntent -= 0.3f;
            }

            //Artificial Friction
            if (rb.velocity.x > 5)
            {
                xIntent -= 1;
            }
            if (rb.velocity.x < -5)
            {
                xIntent += 1;
            }
            if (rb.velocity.y > 5)
            {
                yIntent -= 0.5f;
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
            rb.gravityScale = 0;
            scriptThrough.MoveThrough();
        }
    }

    //If the player hits a wall while hooking, they'll fall.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(currentState == PlayerState.HOOK)
        {
            currentState = PlayerState.AIRBORNE;
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
        rb.velocity = new Vector2(0,0);
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
