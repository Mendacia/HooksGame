using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killThePlayerOnTriggerStay : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        var PlayerControls = collision.gameObject.GetComponent<PlayerControlsNew>();
        if (PlayerControls != null)
        {
            PlayerControls.Die();
        }
    }
}
