using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControls : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject cursor;
    public GameObject aimBot;
    public GameObject player;
    public PlayerControls playerScript;
    public Transform[] hookTargets;
    public float mouseTargetingRadius = 50f;
    public float playerTargetingRadius = 50f;
    public float realTargetingRadius;
    public LayerMask HookTest;
    public bool canHook = false;

    private void Update()
    {
        //Makes a mouse cursor out of a game object
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3 (mousePosition.x, mousePosition.y, 0);

        if((playerScript.currentVelocity/50) > 1)
        {
            realTargetingRadius = (playerScript.currentVelocity / 50) * playerTargetingRadius;
        }
        else
        {
            realTargetingRadius = playerTargetingRadius;
        }


        //Selects the closest hook location
        if (playerScript.CanRetarget())
        {
            var selectedTarget = GetClosestHook(hookTargets);
            if (selectedTarget == null)
            {
                var vector = cursor.transform.position - player.transform.position;
                if (vector.magnitude < realTargetingRadius)
                {
                    aimBot.transform.position = cursor.transform.position;
                }
                else
                {
                    aimBot.transform.position = player.transform.position + vector.normalized * realTargetingRadius;
                }
                canHook = false;
            }
            else
            {
                var line = aimBot.GetComponent<LineRenderer>();
                line.positionCount = 2;
                aimBot.transform.position = new Vector3(selectedTarget.position.x, selectedTarget.position.y, 0);
                var positions = new List<Vector3>();
                positions.Add(aimBot.transform.position);
                positions.Add(player.transform.position);
                line.SetPositions(positions.ToArray());
                canHook = true;
            }
        }
     
    }





    //Code that actually runs when detecting the closest hook location
    Transform GetClosestHook(Transform[] targets)
    {
        //Setting up variables
        Transform bestTarget = null;
        float closestDistanceSqr = mouseTargetingRadius;
        Vector3 currentPosition = cursor.transform.position;

        foreach (Transform potentialTarget in targets)
        {
            //Raycast that looks for walls and hooks
            var hookFindingLazer = Physics2D.Raycast(player.transform.position, potentialTarget.transform.position - player.transform.position, realTargetingRadius, HookTest);
            //Debug Ray Drawing
            //Debug.DrawRay(player.transform.position, potentialTarget.transform.position - player.transform.position, Color.green, 0.017f);
            if (hookFindingLazer.collider == null)
            {
                //Missed
                continue;
            }
            if(hookFindingLazer.collider.gameObject.tag == "Wall" || hookFindingLazer.collider.gameObject.tag == "Ground")
            {
                //Hit Wall
                continue;
            }
            if (hookFindingLazer.collider.gameObject.tag == "Hook")
            {
                Vector2 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
        }

        return bestTarget;
    }

}
