using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookThrough : MonoBehaviour
{

    private Rigidbody2D rb;
    public PlayerControls movementScript;
    public CursorControls cursorScript;
    private Vector3 destination;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DestinationSetter()
    {
        destination = cursorScript.aimBot.transform.position;
    }

    public void MoveThrough()
    {
        //Turns off gravity for the duration of the hook
        rb.gravityScale = 0;
        //He goin
        rb.velocity = (destination - gameObject.transform.position).normalized * 50;
        //Exiting the hook function
        if ((destination - gameObject.transform.position).magnitude < (rb.velocity.magnitude*Time.deltaTime))
        {
            if (Input.GetMouseButton(0))
            {
                movementScript.LeaveHookThrough();
            }
            else
            {
                movementScript.LeaveHookDrop();
            }
        }
    }
}
