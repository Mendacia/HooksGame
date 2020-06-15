using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using GameAnalyticsSDK;

public class PlayerControls : MonoBehaviour
{
    public KeyCode left, right, jump;
    public bool isGrounded;
    public bool goingThrough;
    private Rigidbody2D rb;
    Vector2 wantedDirection;
    private HookThrough hookControlScript;
    private Animator anim;
    public PauseMenu PauseScript;
    public CursorControls cursorControlScript;
    public float aerialSpeedCap = 5;
    public Transform currentCheckpoint;
    public float currentVelocity;
    public bool rPressed;
    public bool lPressed;
    public GameObject deadFucker;
    private Transform animSprite;
    //Making a referencable list of player states
    private enum PlayerState
    {
        GROUNDED,
        AIRBORNE,
        HOOK,
        SWING,
        DYING
    }
    private PlayerState currentState;

    //Rigidbody setup and GameAnalytics setup
    private void Start()
    {
        //accessing the rigidbody
        rb = GetComponent<Rigidbody2D>();
        hookControlScript = GetComponent<HookThrough>();
        anim = GetComponentInChildren<Animator>();
        animSprite = gameObject.transform.Find("Sprite").GetComponent<Transform>();

        GameAnalytics.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cursorControlScript.isPaused == false && Time.timeScale == 1)
            {
                PauseScript.OpenMenu();
                cursorControlScript.PauseGame();
            }
            else if(Time.timeScale == 0)
            {
                PauseScript.MenuContinueGame();
                cursorControlScript.ContinueGame();
            }
        }

        if (currentState == PlayerState.DYING)
        {
            anim.SetBool("isDead", true);
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0, 10 * Time.unscaledDeltaTime);
            if (Input.anyKeyDown)
            {
                Instantiate(deadFucker, gameObject.transform.position, gameObject.transform.rotation);
                gameObject.transform.position = currentCheckpoint.transform.position;
                cursorControlScript.ContinueGame();
                KillAll();
            }
            return;
        }
        else
        {
            anim.SetBool("isDead", false);
        }

        //Setting up variables for moving the player later based on inputs
        var xIntent = rb.velocity.x;
        var yIntent = rb.velocity.y;

        if (Input.GetKeyDown(KeyCode.R))
        {
            KillAll();
            gameObject.transform.position = currentCheckpoint.transform.position;
        }

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

            anim.SetBool("grounded", true);
        }
        else
        {
            anim.SetBool("grounded", false);
        }

        if (currentState == PlayerState.AIRBORNE)
        {
            rb.gravityScale = 5;
            //Taking inputs for LR player movement intent
            if (Input.GetKey(right) && rb.velocity.x < aerialSpeedCap)
            {
                xIntent += 0.5f;
            }
            if (Input.GetKey(left) && rb.velocity.x > -aerialSpeedCap)
            {
                xIntent -= 0.5f;
            }

            //Artificial Friction
            if (rb.velocity.x > aerialSpeedCap)
            {
                xIntent -= 1;
            }
            if (rb.velocity.x < -aerialSpeedCap)
            {
                xIntent += 1;
            }
            if (rb.velocity.y > aerialSpeedCap)
            {
                yIntent -= 0.5f;
            }
        }

        if (currentState == PlayerState.SWING)
        {
            if (Input.GetKey(right))
            {
                rPressed = true;
            }
            else
            {
                rPressed = false;
            }
            if (Input.GetKey(left))
            {
                lPressed = true;
            }
            else
            {
                lPressed = false;
            }
        }

        if(xIntent != 0)
        {
            if(rb.velocity.x < 0)
            {
                animSprite.localScale = new Vector3(-1,1,1);
            }
            if(rb.velocity.x > 0)
            {
                animSprite.localScale = new Vector3(1, 1, 1);
            }
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        if(rb.velocity.y > 0)
        {
            anim.SetBool("movingUp", true);
        }
        if (rb.velocity.y < 0)
        {
            anim.SetBool("movingUp", false);
        }
        currentVelocity = rb.velocity.magnitude;

        //Actually moving the player
        wantedDirection = new Vector2(xIntent, yIntent);
        rb.velocity = wantedDirection;


        //Hook Controls
        if (currentState == PlayerState.GROUNDED && Time.timeScale == 1  || currentState == PlayerState.AIRBORNE && Time.timeScale == 1)
        {
            //Through
            if (Input.GetMouseButtonDown(0) && hookControlScript.CanHook())
            {
                hookControlScript.DestinationSetter();
                currentState = PlayerState.HOOK;
                goingThrough = true;
            }
            //To
            if (Input.GetMouseButtonDown(2) && hookControlScript.CanHook())
            {
                hookControlScript.DestinationSetter();
                currentState = PlayerState.HOOK;
                goingThrough = false;
            }
        }

        //Running the script for Hooking and Swinging code
        if (currentState == PlayerState.HOOK)
        {
            rb.gravityScale = 0;
            hookControlScript.MoveThrough();
        }
        if (currentState == PlayerState.SWING)
        {
            rb.gravityScale = 0;
            hookControlScript.Swing();
        }
    }

    private void FixedUpdate()
    {
        //Swing
        if (Input.GetMouseButtonDown(1) && hookControlScript.CanHook())
        {
            hookControlScript.DestinationSetter();
            currentState = PlayerState.SWING;
        }
    }

    //If the player hits a wall while hooking, they'll fall.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(currentState == PlayerState.HOOK)
        {
            currentState = PlayerState.AIRBORNE;
        }
        if(currentState == PlayerState.SWING)
        {
            KillAll();
        }
    }


    //Leaving hook styles 
    public void LeaveHookThrough()
    {
        //Note that SWING leaves through here too
        currentState = PlayerState.AIRBORNE;
        hookControlScript.currentlySwinging = false;
    }
    public void LeaveHookDrop()
    {
        currentState = PlayerState.AIRBORNE;
        rb.velocity = Vector2.zero;
        rb.position = hookControlScript.destination;
    }


    //Just for swinging, makes sure that while you're attached, the aimbot doesn't move
    public bool CanRetarget()
    {
        if(hookControlScript.currentlySwinging == false && hookControlScript.currentlyGoingTo == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    //referenced in GroundCheck, basically referencing a trigger collider at the player's feet that tells the controller when the player is touching the ground
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

    //Call on another script to kill the player
    public void Die()
    {
        currentState = PlayerState.DYING;
    }

    public void KillAll()
    {
        currentState = PlayerState.AIRBORNE;
        isGrounded = false;
        goingThrough = false;
        hookControlScript.currentlyGoingTo = false;
        currentVelocity = 0;
        rPressed = false;
        lPressed = false;
        hookControlScript.currentlySwinging = false;
        hookControlScript.rotationalSpeed = 0;
        rb.velocity = Vector2.zero;
        cursorControlScript.canHook = false;
        cursorControlScript.aimBot.transform.position = cursorControlScript.cursor.transform.position;
        hookControlScript.Killhook();
    }
}
