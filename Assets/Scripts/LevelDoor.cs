using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public GameObject player;
    public string nextScene;


    private void Update()
    {
        if (gameObject.transform.position == player.transform.position)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
