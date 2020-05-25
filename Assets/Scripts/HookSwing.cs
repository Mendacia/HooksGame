using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSwing : MonoBehaviour
{
    //How the fuck am I going to do this shit? I don't even know where to start

    public GameObject orbiter;
    public GameObject centre;
    public float rotationalVelocity;


    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            orbiter.transform.SetParent(centre.transform);
            centre.transform.Rotate(0, 0, 2);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            orbiter.transform.SetParent(null);
        }
    }
}
