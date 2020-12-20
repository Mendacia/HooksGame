using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator = null;
    private string currentAnimation;
    private Vector3 myScale;
    [SerializeField] private bool wasGroundedLastFrame = true;
    [SerializeField] private bool switcher = false;

    const string PLAYER_IDLE = "ScholarIdle";
    const string PLAYER_WALK = "ScholarWalk";
    const string PLAYER_JUMP = "ScholarTakeOff";
    const string PLAYER_ASCENDING = "ScholarJumping";
    const string PLAYER_FALLING = "ScholarFalling";

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


    public void PlayJumpingAnimation(bool isMovingRight, bool isntMoving, bool isMovingUp)
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

        if (isMovingUp == true && switcher == true)
        {
            playerAnimator.Play(PLAYER_ASCENDING);
            switcher = false;
        }
        if (isMovingUp == false && switcher == false)
        {
            playerAnimator.Play(PLAYER_FALLING);
            switcher = true;
        }
    }
}
