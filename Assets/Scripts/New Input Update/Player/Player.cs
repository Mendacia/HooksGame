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

    private Transform child;
    private Vector2 workspace;
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
        if(Mathf.Sign(RB.velocity.x) != Mathf.Sign(accelerationAndDirection.x))
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
