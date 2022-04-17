using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    #region State Variables
    public CursorState CurrentState {get; private set;}
    public CursorInputHandler InputHandler { get; private set; }
    #endregion
    #region Variables
    public bool usingController;
    public Transform rhetical;
    [SerializeField] private SpriteRenderer rheticalSprite;
    [SerializeField] private Sprite rheticalDefault, rheticalTargeting;
    [SerializeField] private Transform targeter;
    [SerializeField] private SpriteRenderer targeterSprite;
    [SerializeField] private Sprite targeterDefault, targeterTargeting;
    [SerializeField] private Camera currentCam;
    [SerializeField] private Vector2 cursorPos;
    [SerializeField] private Player playerStateMachineRootScript;
    public Transform player;
    [SerializeField] private float targetingRadius;
    public LayerMask hookTargetingLayerMask;

    private bool wantDisabledRhetical = false;
    private bool wantDisabledTargeter = false;
    #endregion
    #region Hooks List
    [SerializeField] private Transform hooksFolder;
    public List<Transform> hooks = new List<Transform>();
    #endregion

    void Start()
    {
        foreach(Transform child in hooksFolder)
        {
            if(child.tag == "Hook")
            {
                hooks.Add(child);
            }
        }
        InputHandler = gameObject.GetComponent<CursorInputHandler>();
        Cursor.visible = false;
        Initialize(new CursorTargetingState("targeting"));
    }
    void Update()
    {
        CurrentState.LogicUpdate(this);
        if (wantDisabledRhetical && usingController)
        {
            if (Vector2.Distance(new Vector2(player.position.x, player.position.y), new Vector2(rhetical.position.x, rhetical.position.y)) < 1)
            {
                rheticalSprite.enabled = false;
            }
        }
        if (wantDisabledTargeter && usingController)
        {
            if (Vector2.Distance(new Vector2(player.position.x, player.position.y), new Vector2(targeter.position.x, targeter.position.y)) < 1)
            {
                targeterSprite.enabled = false;
            }
        }
    }
    void FixedUpdate()
    {
        CurrentState.PhysicsUpdate(this);
    }
    public void ChangeState(CursorState newState)
    {
        CurrentState.Exit(this);
        CurrentState = newState;
        CurrentState.Enter(this);
    }
    public void Initialize(CursorState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter(this);
    }

    public Transform FindHookEligibility()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = /*Change this*/30;
        foreach (Transform hook in hooks)
        {
            var detectionRayCast = Physics2D.Raycast(player.position, hook.position - player.transform.position, 10/*Change this*/, hookTargetingLayerMask);
            if(detectionRayCast.collider == null)
            {
                /*missed*/ Debug.DrawRay(player.position, hook.position - player.transform.position, Color.cyan);
                continue;
            }
            if (detectionRayCast.collider.gameObject.tag == "Wall" || detectionRayCast.collider.gameObject.tag == "Ground")
            {
                /*Hit Wall*/
                Debug.DrawRay(player.position, hook.position - player.transform.position, Color.red);
                continue;
            }
            if(detectionRayCast.collider.transform == hook)
            {
                Debug.DrawRay(player.position, hook.position - player.transform.position, Color.yellow);
                Vector2 directionToTarget = hook.position - rhetical.transform.position;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = hook;
                }
            }
        }
        return bestTarget;
    }

    public void TargeterFollowRhetical()
    {
        targeter.position = rhetical.position;
        rheticalSprite.sprite = rheticalDefault;
        targeterSprite.sprite = targeterDefault;
        targeterSprite.GetComponent<Animator>().SetBool("Targeting", false);
    }
    public void TargeterFollowTarget(Vector2 target)
    {
        targeter.position = Vector2.Lerp(targeter.position, target, 15 * Time.deltaTime);
        rheticalSprite.sprite = rheticalTargeting;
        targeterSprite.sprite = targeterTargeting;
        targeterSprite.GetComponent<Animator>().SetBool("Targeting", true);
    }
    public void TargeterReturnToRhetical()
    {
        targeter.position = Vector2.Lerp(targeter.position, rhetical.position, 15 * Time.deltaTime);
        rheticalSprite.sprite = rheticalDefault;
        targeterSprite.sprite = targeterDefault;
        targeterSprite.GetComponent<Animator>().SetBool("Targeting", false);
        if(rheticalSprite.enabled == false)
        {
            wantDisabledTargeter = true;
        }
    }
    public void ChangeControls(bool isGamepad)
    {
        if (isGamepad)
        {
            Debug.Log("Swapped to pad");
            usingController = true;
            rhetical.position = player.position;
        }
        else
        {
            Debug.Log("Swapped to mouse");
            usingController = false;
            rhetical.position = player.position;
        }
    }
    public void OnMousePosChange(Vector2 mousePos)
    {
        cursorPos = mousePos;
        wantDisabledRhetical = false;
        wantDisabledTargeter = false;
        rheticalSprite.enabled = true;
        targeterSprite.enabled = true;
        if (usingController && mousePos == Vector2.zero)
        {
            wantDisabledRhetical = true;
        }
    }
    public void MoveCursorGamepad()
    {
        Vector2 pPos = new Vector2(player.position.x, player.position.y);
        Vector2 cPos = new Vector2(cursorPos.x, cursorPos.y);
        rhetical.position = Vector2.Lerp(rhetical.position, pPos + cPos * targetingRadius, 20f * Time.deltaTime);
    }
    public void MoveCursorMouse()
    {
        rhetical.position = currentCam.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, 100));
    }
    public void SetRheticalVisibility(bool visible)
    {
        if (visible)
        {
            wantDisabledRhetical = false;
            rheticalSprite.enabled = true;
        }
        else
        {
            wantDisabledRhetical = true;
            rheticalSprite.enabled = false;
        }
    }
    public void SetTargeterVisibility(bool visible)
    {
        if (visible)
        {
            wantDisabledTargeter = false;
            targeterSprite.enabled = true;
        }
        else
        {
            wantDisabledTargeter = true;
            targeterSprite.enabled = false;
        }
    }

    public void SetThePlayerStateToHookThroughState(Vector2 targetPosition)
    {
        playerStateMachineRootScript.ChangeState(new PlayerHookThroughState("hookThrough", targetPosition));
    }
    public void AttemptToStartPreSpinState(Transform targetHook)
    {
        playerStateMachineRootScript.AttemptSwingEntry(targetHook, 0, false);
    }
    public void RemoveThePlayerFromPreSpinState()
    {
        InputHandler.UseSwingInput();
        playerStateMachineRootScript.SwingExit();
    }
    public bool CheckIfPlayerIsInHookThrough()
    {
        if (playerStateMachineRootScript.inHookThrough || playerStateMachineRootScript.inSpinOrPreSpin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
