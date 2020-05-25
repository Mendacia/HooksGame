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



    public bool CanHook()
    {
        return cursorScript.canHook;
    } 


    public void DestinationSetter()
    {
        if (cursorScript.aimBot.transform.position != cursorScript.cursor.transform.position)
        {
            destination = cursorScript.aimBot.transform.position;
        }
    }

    public void MoveThrough()
    {
        //Tests if the player will move through the hook on the next frame
        if ((destination - gameObject.transform.position).magnitude < (rb.velocity.magnitude * Time.deltaTime))
        {
            //Tests the input type and tells PlayerControls to act accordingly
            if (movementScript.goingThrough == true)
            {
                movementScript.LeaveHookThrough();
            }
            else if (movementScript.goingThrough == false)
            {
                movementScript.LeaveHookDrop();
            }
        }
        //This needs to be in the else statement otherwise the player is occasionally sent back the way they came when exiting via LeaveHookThrough
        else
        {
            //Turns off gravity for the duration of the hook
            rb.gravityScale = 0;
            //He goin
            rb.velocity = (destination - gameObject.transform.position).normalized * 50;
        }
    }
}
