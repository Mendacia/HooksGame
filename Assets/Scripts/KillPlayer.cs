using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        var PlayerControls= collision.gameObject.GetComponent<PlayerControls>();
        if (PlayerControls != null)
        {
            PlayerControls.Die();
        }
    }
}
