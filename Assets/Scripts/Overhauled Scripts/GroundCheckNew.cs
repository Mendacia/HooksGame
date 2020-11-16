using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckNew : MonoBehaviour
{
    public PlayerControlsNew player;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.SetGrounded(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            player.SetGrounded(false);
        }
    }
}
