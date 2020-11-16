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
    [SerializeField] private float playerTargetingRange = 50;
    [SerializeField] private float cursorTargetingRange = 50;
    [SerializeField] private GameObject aimIndicatorObject = null;
    private float updatedTargetingRange = 0;


    private void Update()
    {
        //This puts the rhetical on top of the player's mouse.
        var currentMouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hooksScript. updateCursorLocationInHookManagerScript(currentMouseLocation);
        cursorObject.transform.position = new Vector3(currentMouseLocation.x, currentMouseLocation.y, 0);

        UpdateCursorSnappingRadius();
        MoveTheCursorTargetingSnapperToTheNearestInRangeHook();
    }

    private void UpdateCursorSnappingRadius()
    {
        if ((player.GetPlayerVelocity() / 50) > 1)
        {
            updatedTargetingRange = (player.GetPlayerVelocity() / 50) * playerTargetingRange;
        }
        else
        {
            updatedTargetingRange = playerTargetingRange;
        }
        hooksScript.TellHookManagerWhatTheTargetingRadiusAre(updatedTargetingRange, cursorTargetingRange);
    }

    private void MoveTheCursorTargetingSnapperToTheNearestInRangeHook()
    {
        var selectedTarget = hooksScript.GetClosestHook(hooksScript.hooks.ToArray());
        if (selectedTarget == null)
        {
            var vector = cursorObject.transform.position - player.transform.position;
            if (vector.magnitude < updatedTargetingRange)
            {
                cursorTargetingSnapper.transform.position = cursorObject.transform.position;
            }
            else
            {
                cursorTargetingSnapper.transform.position = player.transform.position + vector.normalized * updatedTargetingRange;
            }
            player.CanPlayerHook(false);
            aimIndicatorObject.GetComponent<Animator>().SetBool("ShouldRotate", false);
        }
        else
        {
            cursorTargetingSnapper.transform.position = new Vector3(selectedTarget.position.x, selectedTarget.position.y, 0);
            player.CanPlayerHook(true);
            aimIndicatorObject.GetComponent<Animator>().SetBool("ShouldRotate", true);
        }
        hookController.giveTheHookControllerTheSelectedTarget(selectedTarget);
    }
}
