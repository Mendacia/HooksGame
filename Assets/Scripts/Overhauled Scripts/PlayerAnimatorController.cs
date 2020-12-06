using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator = null;
    private string currentAnimation;
    private Vector3 myScale;
    private bool wasGroundedLastFrame = true;

    const string PLAYER_IDLE = "ScholarIdle";
    const string PLAYER_WALK = "ScholarWalk";
    const string PLAYER_JUMP = "ScholarTakeOff";

    private void Start()
    {
        myScale = playerAnimator.gameObject.transform.localScale;
    }

    public void PlayIdleAnimation()
    {
        playerAnimator.Play(PLAYER_IDLE);
        wasGroundedLastFrame = true;
    }

    public void PlayWalkingAnimation(bool isMovingRight)
    {

        playerAnimator.Play(PLAYER_WALK);
        if (isMovingRight)
        {
            playerAnimator.gameObject.transform.localScale = new Vector3(-myScale.x, myScale.y, myScale.z);
        }
        if (!isMovingRight)
        {
            playerAnimator.gameObject.transform.localScale = myScale;
        }
        wasGroundedLastFrame = true;
    }

    public void PlayJumpingAnimation(bool isMovingRight, bool isntMoving)
    {
        if (wasGroundedLastFrame)
        {
            playerAnimator.Play(PLAYER_JUMP);
            wasGroundedLastFrame = false;
        }
        if (isntMoving)
        {
            //this only exists to let the anim for jump play if the player isn't moving at all.
        }
        else if (isMovingRight)
        {
            playerAnimator.gameObject.transform.localScale = new Vector3(-myScale.x, myScale.y, myScale.z);
        }
        else  if (!isMovingRight)
        {
            playerAnimator.gameObject.transform.localScale = myScale;
        }
    }
}
