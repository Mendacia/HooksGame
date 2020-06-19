using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stoptimer : MonoBehaviour
{
    public HUD PlayerHUD;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHUD.updateTimePlease = false;
        }

    }
}
