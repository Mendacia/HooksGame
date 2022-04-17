using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables

    public PlayerState CurrentState { get; private set; }
    public PlayerData playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField] private Transform groundCheck;
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    public bool inHookThrough = false;
    public bool inSpinOrPreSpin = false;
    public bool inAir = false;
    private Transform child;
    private Vector2 workspace;

    private GameObject SwingChainRoot = null;
    private GameObject spinRoot = null;
    private GameObject spinPlayer = null;
    public float spinSpeed = 0f;
    private LineRenderer currentRope;
    #endregion

    #region Unity Callback Functions

    void Start()
    {
        child = GetComponentInChildren<Transform>();
        Anim = GetComponentInChildren<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();

        FacingDirection = -1;

        Initialize(new PlayerIdleState("idle"));
    }

    void Update()
    {
        CurrentVelocity = RB.velocity;
        CurrentState.LogicUpdate(this);
    }

    void FixedUpdate()
    {
        CurrentState.PhysicsUpdate(this);
    }
    #endregion

    #region Set Functions
    public void SetAccelerationX(Vector2 accelerationAndDirection, float updatedMovespeedCap)
    {
        if(CurrentVelocity.x < updatedMovespeedCap && CurrentVelocity.x > -updatedMovespeedCap)
        {
            RB.velocity += accelerationAndDirection * Time.deltaTime;
        }
        if(accelerationAndDirection.x != 0 && Mathf.Sign(RB.velocity.x) != Mathf.Sign(accelerationAndDirection.x))
        {
            RB.velocity *= new Vector2(0.8f, 1);
        }
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityHook(Vector2 destination, float velocity)
    {
        RB.velocity = (destination - RB.position).normalized * velocity;
    }

    public void StopThePlayer()
    {
        RB.velocity *= new Vector2(0.8f, 1);
    }

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter(this);
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit(this);
        CurrentState = newState;
        CurrentState.Enter(this);
    }

    public void AttemptSwingEntry(Transform targetHook, float radius, bool ignore)
    {
        if (inAir || ignore)
        {
            ChangeState(new PlayerPreSpinState("preSpinState", targetHook, radius, ignore));
        }
    }
    public void SwingExit()
    {
        Destroy(SwingChainRoot);
        Destroy(spinRoot);
        ChangeState(new PlayerInAirState("inAir", false));
    }
    public void SpinEntry(Transform targetHook)
    {
        spinRoot = Instantiate(playerData.spinAxis, targetHook.position, Quaternion.identity);
        spinPlayer = Instantiate(playerData.spinAnchor, transform.position, Quaternion.identity);
        spinPlayer.transform.SetParent(spinRoot.transform);
    }
    public void SpinInitialVelocity(float rotationalVelocity)
    {
        spinSpeed = rotationalVelocity;
    }
    public void OrbitalAcceleration(float accelerationIntent)
    {
        if (accelerationIntent != 0)
        {
            spinSpeed = spinSpeed + (playerData.orbitalAcceleration * accelerationIntent) * Time.deltaTime;
        }
        spinSpeed = Mathf.Clamp(spinSpeed, -(playerData.orbitalSpeedCap), playerData.orbitalSpeedCap);
        spinRoot.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        RB.velocity = (new Vector2(spinPlayer.transform.position.x, spinPlayer.transform.position.y) - new Vector2(transform.position.x, transform.position.y)) / Time.deltaTime;
        RB.MovePosition(spinPlayer.transform.position);
    }
    public void AttachRope(Transform targetHook, float radius)
    {
        GameObject hookEnd = Instantiate(playerData.swingChainRoot, targetHook);
        Vector2 dir = hookEnd.transform.position - transform.position;
        dir = hookEnd.transform.InverseTransformDirection(dir);
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) +270;

        hookEnd.transform.rotation = Quaternion.Euler(0, 0, angle); //This has to be done to instantiate the rest of the chain at the right rotation.
        int roundedRadius = (Mathf.RoundToInt(radius)) * 2; //Radius is set in PreSpinState on entry so the desired length is set correctly, rather than when the player reaches the other side, which could otherwise lead to shorter or longer than desired chains.
        float remainder = ((radius*2) - 0.25f) / roundedRadius; //Remainder will be used to shrink/expand each chain link a tiny bit so the length is exactly what the player asked for.

        Transform currentTarget = hookEnd.transform;
        for(int i = 0; i < roundedRadius; i++) //Since you're an idiot Nem:     Initiation; Condition; Action on Completion;      Stop forgetting.
        {
            GameObject hookLink = Instantiate(playerData.swingChainLink, hookEnd.transform);
            hookLink.transform.localScale *= remainder;
            hookLink.transform.localPosition = new Vector2(0, (-i/2) -0.25f ); //Finicky positioning of each link
            if(currentTarget == hookEnd.transform) //First Link, need to access Distance Joint 2D
            {
                currentTarget.GetComponent<DistanceJoint2D>().connectedBody = hookLink.GetComponent<Rigidbody2D>();
            }
            else if(i == roundedRadius - 1) //Accesses the final link and attaches it to the player
            {
                currentTarget.GetComponent<HingeJoint2D>().connectedBody = hookLink.GetComponent<Rigidbody2D>();
                hookLink.GetComponent<HingeJoint2D>().connectedBody = transform.GetComponent<Rigidbody2D>();
            }
            else //Every other link
            {
                currentTarget.GetComponent<HingeJoint2D>().connectedBody = hookLink.GetComponent<Rigidbody2D>();
            }
            currentTarget = hookLink.transform;
        }
        SwingChainRoot = hookEnd;
    }

    public void DrawRope(Vector2 target)
    {
        currentRope = Instantiate(playerData.hookLineRenderer).GetComponent<LineRenderer>();
        currentRope.positionCount = 2;
        currentRope.SetPosition(1, transform.position);
        currentRope.SetPosition(0, target);
    }
    public void UpdateRope(Vector2 target)
    {
        currentRope.SetPosition(1, transform.position);
    }
    public void DestroyRope()
    {
        Destroy(currentRope);
    }

    #endregion

    #region Check Funtions
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Other Functions
    public void AnimationTrigger() => CurrentState.AnimationTrigger(this);
    public void AnimationFinishTrigger() => CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        child.Rotate(0, 180, 0);
    }
    #endregion
}
