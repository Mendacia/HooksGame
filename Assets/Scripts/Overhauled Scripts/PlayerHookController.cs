﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Set these up please and thankyou")]
    private PlayerControlsNew player = null;
    [SerializeField] private InputGod inputScript = null;
    [SerializeField] private PlayerHookVisuals hookVisualsScript = null;
    [SerializeField] private AudioClip chainClinkSound = null;

    [Header("Variables for swing function, these are scary, name them better later")]
    [SerializeField] private float rotationalSpeed = 5;
    [SerializeField] private float maximumRotationalSpeed = 100;
    [SerializeField] private float rotationalAcceleration = 5;
    private float accelerationIntent = 0f;

    [Header("Variables for through and to functions")]
    [SerializeField] private float hookingSpeed = 50;

    [System.NonSerialized] public Vector2 destination;
    [System.NonSerialized] public bool playerIsHookingThrough = false;
    private Rigidbody2D myRigidbody = null;
    private Transform selectedTarget = null;
    private GameObject swingAnchor;
    private GameObject selectedTargetAnchor;
    private bool goClockwise = true;
    [SerializeField] private bool shouldDrawHook = false;

    private AudioSource myAudioScource;
    private GameObject myAnchor = null;
    private GameObject mySelectedTargetAnchor = null;
    private bool overrideSwingControls = false;

    [SerializeField] private float myRadius = 0;

    private void Start()
    {
        myAudioScource = gameObject.GetComponent<AudioSource>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        swingAnchor = new GameObject();
        selectedTargetAnchor = new GameObject();
        player = gameObject.GetComponent<PlayerControlsNew>();
    }

    private void Update()
    {
        if (shouldDrawHook)
        {
            hookVisualsScript.DrawHook();
        }
    }

    public void giveTheHookControllerTheSelectedTarget(Transform theTarget)
    {
        selectedTarget = theTarget;
    }

    public void InitiateHook()
    {
        if (selectedTarget != null)
        {
            destination = selectedTarget.position;
        }

        if (Mathf.Abs(myRigidbody.velocity.x * 1.5f) > Mathf.Abs(myRigidbody.velocity.y))
        {
            Debug.Log("Higher X Speed");
            var xSigned = Mathf.Sign(myRigidbody.velocity.x);
            if (myRigidbody.position.y > destination.y)
            {
                if (xSigned > 0)
                {
                    goClockwise = true;
                    if (Input.GetKey(inputScript.right))
                    {
                        overrideSwingControls = true;
                    }
                }
                else
                {
                    goClockwise = false;
                    if (Input.GetKey(inputScript.left))
                    {
                        overrideSwingControls = true;
                    }
                }
            }
            else
            {
                if (xSigned < 0)
                {
                    goClockwise = true;
                    if (Input.GetKey(inputScript.left))
                    {
                        overrideSwingControls = false;
                    }
                }
                else
                {
                    goClockwise = false;
                    if (Input.GetKey(inputScript.left))
                    {
                        overrideSwingControls = false;
                    }

                }
            }
        }
        else
        {
            Debug.Log("Higher Y Speed");
            var ySigned = Mathf.Sign(myRigidbody.velocity.y);
            if (myRigidbody.position.x > destination.x)
            {
                if (ySigned > 0)
                {
                    goClockwise = false;
                    if (Input.GetKey(inputScript.right))
                    {
                        overrideSwingControls = false;
                    }
                }
                else
                {
                    goClockwise = true;
                    if (Input.GetKey(inputScript.left))
                    {
                        overrideSwingControls = false;
                    }
                }
            }
            else
            {
                if (ySigned < 0)
                {
                    goClockwise = false;
                    if (Input.GetKey(inputScript.left))
                    {
                        overrideSwingControls = false;
                    }
                }
                else
                {
                    goClockwise = true;
                    if (Input.GetKey(inputScript.left))
                    {
                        overrideSwingControls = false;
                    }
                }
            }
        }
        if(!Input.GetKey(inputScript.left) && !Input.GetKey(inputScript.right))
        {
            overrideSwingControls = false;
        }

        // Find Tangent Speed
        var radius = Vector2.Distance(destination, myRigidbody.position);
        myRadius = radius;


        rotationalSpeed = (((myRigidbody.velocity.magnitude / radius) / (Mathf.PI/30)) * 6) * (goClockwise ? -1 : 1);

        myAudioScource.PlayOneShot(chainClinkSound);
        StartCoroutine(Hitfreeze());
    }

    public void MoveThrough()
    {
        //Tests if the player will move through the hook on the next frame
        if (((destination - myRigidbody.position).magnitude < (myRigidbody.velocity.magnitude * Time.deltaTime)) || destination == new Vector2(transform.position.x, transform.position.y))
        {
            if (playerIsHookingThrough)
            {
                player.LeaveHookThrough();
            }
            else if (!playerIsHookingThrough)
            {
                player.LeaveHookDrop();
            }
            hookVisualsScript.KillHook();
        }
        else
        {
            myRigidbody.gravityScale = 0;
            //This is where the player actually finally moves
            myRigidbody.velocity = (destination - myRigidbody.position).normalized * hookingSpeed;
            shouldDrawHook = true;
            myAudioScource.PlayOneShot(chainClinkSound);
        }
    }

    public void SwingSetup()
    {
        //Spawns 2 objects, 1 which the player will follow and will be parented to the other, and the other stays at the selected target
        myAnchor = Instantiate(swingAnchor, player.transform.position, Quaternion.identity);
        mySelectedTargetAnchor = Instantiate(selectedTargetAnchor, selectedTarget.position, Quaternion.identity);
        myAnchor.transform.SetParent(mySelectedTargetAnchor.transform);
    }

    public void Swing()
    {
        if (!overrideSwingControls)
        {
            if (!Input.GetKey(inputScript.left) && !Input.GetKey(inputScript.right))
            {
                accelerationIntent = 0;
            }
            else if (Input.GetKey(inputScript.left))
            {
                accelerationIntent += -1;
            }
            else if (Input.GetKey(inputScript.right))
            {
                accelerationIntent += 1;
            }
        }
        else if (overrideSwingControls)
        {
            if (!Input.GetKey(inputScript.right) && !Input.GetKey(inputScript.left))
            {
                accelerationIntent = 0;
            }
            else if (Input.GetKey(inputScript.right))
            {
                accelerationIntent += -1;
            }
            else if (Input.GetKey(inputScript.left))
            {
                accelerationIntent += 1;
            }
        }

        if (accelerationIntent != 0)
        {
            rotationalSpeed = rotationalSpeed + (rotationalAcceleration * accelerationIntent) * Time.deltaTime;
            accelerationIntent -= 0.75f;
        }
        rotationalSpeed = Mathf.Clamp(rotationalSpeed, -maximumRotationalSpeed, maximumRotationalSpeed);

        mySelectedTargetAnchor.transform.Rotate(0, 0, rotationalSpeed * Time.deltaTime);
        myRigidbody.velocity = (new Vector2(myAnchor.transform.position.x, myAnchor.transform.position.y) - myRigidbody.position) / Time.deltaTime;
        myRigidbody.MovePosition(myAnchor.transform.position);
        Debug.DrawLine(myRigidbody.position, destination, Color.green);
        shouldDrawHook = true;
        //Watch this intently, it's worrying
        hookVisualsScript.GiveTheHookVisualsScriptTheSelectedTarget(selectedTarget);
    }

    public void SwingKiller(bool isDead)
    {
        myAnchor.transform.SetParent(null);
        Destroy(myAnchor);
        Destroy(mySelectedTargetAnchor);
        accelerationIntent = 0;
        if (!isDead)
        {
            player.LeaveHookThrough();
        }
        hookVisualsScript.KillHook();
    }

    public void killHookVisuals()
    {
        hookVisualsScript.KillHook();
        shouldDrawHook = false;
    }

    IEnumerator Hitfreeze()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(1 / 10f);
        Time.timeScale = 1;
    }
}
