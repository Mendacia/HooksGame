using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectionScript : MonoBehaviour
{
    public HUD playerHUD;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            playerHUD.coinCount++;
        }
    }
}
