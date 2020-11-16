using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorInputControls : MonoBehaviour
{
    [SerializeField] private PlayerControlsNew player = null;
    [SerializeField] private HookManager hooksScript = null;
    [SerializeField] private PlayerHookController hookController;
    [SerializeField] private GameObject cursorObject = null;
    [SerializeField] private GameObject cursorTargetingSnapper = null;
    [SerializeField] private float snappingRadius = 50;
    [SerializeField] private GameObject aimIndicatorObject = null;
    private float updatedSnappingRadius = 0;


    private void Update()
    {
        //This puts the rhetical on top of the player's mouse.
        var currentMouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorObject.transform.position = new Vector3(currentMouseLocation.x, currentMouseLocation.y, 0);

        UpdateCursorSnappingRadius();
        MoveTheCursorTargetingSnapperToTheNearestInRangeHook();
    }

    private void UpdateCursorSnappingRadius()
    {
        if ((player.GetPlayerVelocity() / 50) > 1)
        {
            updatedSnappingRadius = (player.GetPlayerVelocity() / 50) * snappingRadius;
        }
        else
        {
            updatedSnappingRadius = snappingRadius;
        }
        hooksScript.TellHookManagerWhatTheTargetingRadiusAre(snappingRadius, updatedSnappingRadius);
    }

    private void MoveTheCursorTargetingSnapperToTheNearestInRangeHook()
    {
        var selectedTarget = hooksScript.GetClosestHook(hooksScript.hooks.ToArray());
        if (selectedTarget == null)
        {
            var vector = cursorObject.transform.position - player.transform.position;
            if (vector.magnitude<updatedSnappingRadius)
            {
                cursorTargetingSnapper.transform.position = cursorObject.transform.position;
            }
            else
            {
                cursorTargetingSnapper.transform.position = player.transform.position + vector.normalized* updatedSnappingRadius;
            }
            //canHook = false;
            aimIndicatorObject.GetComponent<Animator>().SetBool("ShouldRotate", false);
        }
        else
        {
            cursorTargetingSnapper.transform.position = new Vector3(selectedTarget.position.x, selectedTarget.position.y, 0);
            //canHook = true;
            aimIndicatorObject.GetComponent<Animator>().SetBool("ShouldRotate", true);
        }
        hookController.giveTheHookControllerTheSelectedTarget(selectedTarget);
    }
}
