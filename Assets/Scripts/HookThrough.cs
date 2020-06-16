using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookThrough : MonoBehaviour
{

    private Rigidbody2D rb;
    public PlayerControls movementScript;
    public CursorControls cursorScript;
    public GameObject swingPositionObject;
    public GameObject parentOfSwingAnchor;
    public float rotationalSpeed;
    public float rotationalAcceleration;
    public float rotationalSpeedCap;
    public Vector2 destination;
    public bool currentlySwinging;
    public bool currentlyGoingTo;
    private bool goClockwise;
    private int accelerationIntent;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(currentlySwinging == false)
        {
            swingPositionObject.transform.position = gameObject.transform.position;
        }
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

        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            var xSigned = Mathf.Sign(rb.velocity.x);
            if (rb.position.y > destination.y)
            {
                if (xSigned > 0)
                {
                    goClockwise = true;
                }
                else
                {
                    goClockwise = false;
                }
            }
            else
            {
                if (xSigned < 0)
                {
                    goClockwise = true;
                }
                else
                {
                    goClockwise = false;

                }
            }
        }
        else
        {
            var ySigned = Mathf.Sign(rb.velocity.y);
            if (rb.position.x > destination.x)
            {
                if (ySigned > 0)
                {
                    goClockwise = false;
                } 
                else
                {
                    goClockwise = true;
                 }
            }
            else
            {
                if (ySigned < 0)
                {
                    goClockwise = false;
                }
                else
                {
                    goClockwise = true;
                }
            }
        }

        // Find Tangent Speed
        var radius = Vector2.Distance(destination, rb.position);
        var circumference = Mathf.PI * 2 * radius;

        //rotationalSpeed = rb.velocity.magnitude / circumference * (goClockwise ? -1 : 1) * Mathf.Rad2Deg * 30;
        rotationalSpeed = circumference * (360 / (circumference / rb.velocity.magnitude)) * Time.deltaTime * (goClockwise? -1 : 1);

        StartCoroutine(Hitfreeze());

    }

    public void MoveThrough()
    {
        //Tests if the player will move through the hook on the next frame
        if (((destination - rb.position).magnitude < (rb.velocity.magnitude * Time.deltaTime)) || destination == new Vector2(gameObject.transform.position.x, gameObject.transform.position.y))
        {
            //Tests the input type and tells PlayerControls to act accordingly
            if (movementScript.goingThrough == true)
            {
                movementScript.LeaveHookThrough();
                Killhook();
                currentlyGoingTo = false;
                Debug.DrawLine(rb.position, destination, Color.red);
            }
            else if (movementScript.goingThrough == false)
            {
                movementScript.LeaveHookDrop();
                Killhook();
                currentlyGoingTo = false;
                Debug.DrawLine(rb.position, destination, Color.blue);
            }
        }
        //This needs to be in the else statement otherwise the player is occasionally sent back the way they came when exiting via LeaveHookThrough
        else
        {
            //Turns off gravity for the duration of the hook
            rb.gravityScale = 0;
            //He goin
            rb.velocity = (destination - rb.position).normalized * 50;
            currentlyGoingTo = true;
            HookEffects();
        }
    }

    public void Swing()
    {
        //Sets parenting and stops the object from following the player
        if (swingPositionObject.transform.parent != cursorScript.aimBot.transform)
        {
            swingPositionObject.transform.SetParent(cursorScript.aimBot.transform);
        }
        else
        {
            currentlySwinging = true;
            if (!(movementScript.lPressed || movementScript.rPressed))
            {
                accelerationIntent = 0;
            } else
            {
                if (accelerationIntent == 0)
                {
                    accelerationIntent = 0;
                    if (movementScript.rPressed == true)
                    {
                        if (rb.position.y > destination.y)
                        {
                            accelerationIntent += 1;
                        }
                        else
                        {
                            accelerationIntent -= 1;
                        }
                    }
                    if (movementScript.lPressed == true)
                    {
                        if (rb.position.y > destination.y)
                        {
                            accelerationIntent -= 1;
                        }
                        else
                        {
                            accelerationIntent += 1;
                        }
                    }
                }
            }
            
            if (accelerationIntent == -1)
            {
                rotationalSpeed = rotationalSpeed + rotationalAcceleration * Time.deltaTime;
            }
            if (accelerationIntent == 1)
            {
                rotationalSpeed = rotationalSpeed - rotationalAcceleration * Time.deltaTime;
            }
            rotationalSpeed = Mathf.Clamp(rotationalSpeed, -rotationalSpeedCap, rotationalSpeedCap);

            cursorScript.aimBot.transform.Rotate(0, 0, rotationalSpeed * Time.deltaTime);
            rb.velocity = (new Vector2(swingPositionObject.transform.position.x, swingPositionObject.transform.position.y) - rb.position) / Time.deltaTime;
            rb.MovePosition(swingPositionObject.transform.position);
            Debug.DrawLine(rb.position, destination, Color.green);
            HookEffects();
        }

        if (Input.GetMouseButtonUp(1))
        //stop move.
        {
            currentlySwinging = false;
            swingPositionObject.transform.SetParent(parentOfSwingAnchor.transform);
            movementScript.LeaveHookThrough();
            Killhook();
        }
    }
    void HookEffects()
    {
        if (cursorScript.isPaused == false)
        {
            //Drawing the hook
            var line = cursorScript.aimBot.GetComponent<LineRenderer>();
            cursorScript.aimBot.GetComponent<SpriteRenderer>().enabled = true;
            cursorScript.aimBot.GetComponent<LineRenderer>().enabled = true;
            line.positionCount = 2;
            var positions = new List<Vector3>();
            positions.Add(cursorScript.aimBot.transform.position);
            positions.Add(cursorScript.player.transform.position);
            line.SetPositions(positions.ToArray());
        }
    }

    IEnumerator Hitfreeze()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(1 / 10f);
        Time.timeScale = 1;
    }

    public void Killhook()
    {
        cursorScript.aimBot.GetComponent<SpriteRenderer>().enabled = false;
        cursorScript.aimBot.GetComponent<LineRenderer>().enabled = false;
    }
}
