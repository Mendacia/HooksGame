using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public GameObject player;
    private PlayerControls playerScript;
    public HookThrough hookControlScript;
    public string nextScene;

    private void Start()
    {
        playerScript = player.GetComponent<PlayerControls>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hookControlScript.currentlyGoingTo && playerScript.goingThrough == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
