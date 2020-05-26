using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookSwing : MonoBehaviour
{
    //How the fuck am I going to do this shit? I don't even know where to start

    public GameObject orbiter;
    public GameObject centre;
    public float rotationalVelocity;
    public CursorControls mouseControls;


    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            orbiter.transform.SetParent(mouseControls.aimBot.transform);
            mouseControls.aimBot.transform.Rotate(0, 0, rotationalVelocity);
        }
        if (Input.GetMouseButtonUp(1))
        {
            orbiter.transform.SetParent(null);
        }
    }
}
