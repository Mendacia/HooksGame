using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp: MonoBehaviour
{
    public Transform from;
    public Transform to;
    public float lerpDistance;
    public bool timeDriven;

    //What does this need in order to function?


    private void Update()
    {
        //gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, gameObject.transform.position.z);

        gameObject.transform.position = Vector3.Lerp(from.position, new Vector3 (to.position.x, to.position.y, from.position.z), lerpDistance * (timeDriven? Time.deltaTime:1));
    }
}
