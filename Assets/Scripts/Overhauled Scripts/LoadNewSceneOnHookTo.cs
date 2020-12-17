using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewSceneOnHookTo : MonoBehaviour
{
    private PlayerControlsNew thePlayer;
    [SerializeField] private string sceneToLoad = "";
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            thePlayer = other.GetComponent<PlayerControlsNew>();
            if (thePlayer.ShouldILoadScene())
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
