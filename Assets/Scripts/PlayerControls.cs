using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public KeyCode left, right, jump;
    public bool isGrounded;
    private Rigidbody2D rb;
    Vector2 wantedDirection;

    private void Start()
    {
        //accessing the rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Setting up variables for moving the player later based on inputs
        var xIntent = 0;
        var yIntent = rb.velocity.y;

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
        if (isGrounded)
        {
            if (Input.GetKey(jump))
            {
                yIntent = 10;
            }
        }

        //Actually moving the player
        wantedDirection = new Vector2(xIntent, yIntent);
        rb.velocity = wantedDirection;


        //Hook Controls
    }


    //referenced in GroundCheck, basically referencing a trigger collider at the player's feet that tells the controller when the player is touching the ground
    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}
