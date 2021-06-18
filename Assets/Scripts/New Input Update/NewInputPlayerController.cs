using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInputPlayerController : MonoBehaviour
{
    [SerializeField] private float groundAccelerationSpeed = 1;
    [SerializeField] private float jumpStrength = 100;

    [SerializeField] private float groundXSpeedCap = 20;

    private Rigidbody2D myRigidbody;
    private Vector2 moveDirection;
    private float horizontal;
    private float vertical;
    private bool jump = false;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = (Vector2.up * Vector2.zero + Vector2.right * horizontal);
    }

    void FixedUpdate()
    {
        //Capping player's horizontal speed while grounded
        if(myRigidbody.velocity.x < groundXSpeedCap && myRigidbody.velocity.x > -groundXSpeedCap)
        {
            myRigidbody.velocity += moveDirection * groundAccelerationSpeed * Time.deltaTime;
            Debug.Log("Player should be allowed to move");
        }
        //Slow the player faster if they're moving the oposite direction, or not moving
        if (moveDirection == Vector2.zero || Mathf.Sign(myRigidbody.velocity.x) != Mathf.Sign(moveDirection.x))
        {
            myRigidbody.velocity *= new Vector2(0.8f, 1);
        }
        //Calculating this in fixed because if I don't I get weirdness, also Impulse because it'll be easier to modify
        if (jump)
        {
            myRigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            jump = false;
        }
    }

    public void onMoveInput(float myHorizontal, float myVertical)
    {
        vertical = myVertical;
        horizontal = myHorizontal;
        Debug.Log("Input " + horizontal + ", " + vertical);
    }

    public void onJumpInput()
    {
        jump = true;
    }
}
