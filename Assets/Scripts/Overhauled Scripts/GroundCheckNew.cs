using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckNew : MonoBehaviour
{
    public PlayerControlsNew player;
    private bool iAmStillGroundedTheresJustMoreThanOneColliderHereDickhead;

    private void Update()
    {
        iAmStillGroundedTheresJustMoreThanOneColliderHereDickhead = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.SetGrounded(true);
            iAmStillGroundedTheresJustMoreThanOneColliderHereDickhead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" && iAmStillGroundedTheresJustMoreThanOneColliderHereDickhead == false)
        {
            player.SetGrounded(false);
        }
    }
}
