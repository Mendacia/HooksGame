using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToZero : MonoBehaviour
{
    private PlayerControlsNew thePlayer;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.position = new Vector3(0, 5, 0);
        }
    }
}
