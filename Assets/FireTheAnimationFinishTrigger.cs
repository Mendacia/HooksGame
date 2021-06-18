using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTheAnimationFinishTrigger : MonoBehaviour
{
    [SerializeField] private Player playerScript;

    public void FireIt()
    {
        playerScript.AnimationFinishTrigger();
    }
}
