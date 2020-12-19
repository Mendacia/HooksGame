using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSemisolidWhileHooking : MonoBehaviour
{
    private BoxCollider2D myCollider;
    private PlatformEffector2D myEffector;
    private PlayerControlsNew player;

    private void Awake()
    {
        myCollider = gameObject.GetComponent<BoxCollider2D>();
        //myEffector = gameObject.GetComponent<PlatformEffector2D>();
        player = GameObject.Find("Player Stand In").GetComponent<PlayerControlsNew>();
        player.addMeToTheSemiSolidList(this);
    }

    public void EnablePlatform()
    {
        //myEffector.enabled = true;
        myCollider.enabled = true;
    }

    public void DisablePlatform()
    {
        //myEffector.enabled = true;
        myCollider.enabled = false;
    }
}
