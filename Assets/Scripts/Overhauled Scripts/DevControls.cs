using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevControls : MonoBehaviour
{
    private enum SLOWstate
    {
        NA,
        S,
        L,
        O,
        W,
    }
    private SLOWstate currentSLOWState = SLOWstate.NA;
    [Header("Active Dev Cheats")]
    [Tooltip("Slows time to 20% of regular speed (Cheat code SLOW)")]
    [SerializeField] private bool slowEnabled = false;
    private bool holdThis = false;



    private void Update()
    {
        if (slowEnabled)
        {
            Time.timeScale = 0.2f;
            holdThis = true;
        }
        if (slowEnabled == false && holdThis == true)
        {
            holdThis = false;
            Time.timeScale = 1f;
        }





        //SLOW STATE BEYOND HERE, SLOWS THE GAME TO 1/5th SPEED
        switch (currentSLOWState)
        {
            case SLOWstate.NA:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        currentSLOWState = SLOWstate.S;
                    }
                }
                break;
            case SLOWstate.S:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        currentSLOWState = SLOWstate.L;
                    }
                    else
                    {
                        currentSLOWState = SLOWstate.NA;
                    }
                }
                break;
            case SLOWstate.L:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        currentSLOWState = SLOWstate.O;
                    }
                    else
                    {
                        currentSLOWState = SLOWstate.NA;
                    }
                }
                break;
            case SLOWstate.O:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        currentSLOWState = SLOWstate.W;
                    }
                    else
                    {
                        currentSLOWState = SLOWstate.NA;
                    }
                }
                break;
            case SLOWstate.W:
                slowEnabled = !slowEnabled;
                currentSLOWState = SLOWstate.NA;
                break;
        }

    }
}
