﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControls : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject cursor;
    public GameObject aimBot;
    public GameObject player;
    public Transform[] hookTargets;
    public float mouseTargetingRadius = 50f;
    public float playerTargetingRadius = 50f;
    public LayerMask HookTest;
    public bool canHook = false;

    private void Update()
    {
        //Makes a mouse cursor out of a game object
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3 (mousePosition.x, mousePosition.y, 0);

        //Selects the closest hook location
        var selectedTarget = GetClosestHook(hookTargets);
        if (selectedTarget == null)
        {
            var vector = cursor.transform.position - player.transform.position;
            if (vector.magnitude < playerTargetingRadius)
            {
                aimBot.transform.position = cursor.transform.position;
            }
            else
            {
                aimBot.transform.position = player.transform.position + vector.normalized * playerTargetingRadius;
            }
            canHook = false;
        }
        else
        {
            aimBot.transform.position = new Vector3(selectedTarget.position.x, selectedTarget.position.y, 0);
            canHook = true;
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
            var hookFindingLazer = Physics2D.Raycast(player.transform.position, potentialTarget.transform.position - player.transform.position, playerTargetingRadius, HookTest);
            Debug.DrawRay(player.transform.position, potentialTarget.transform.position - player.transform.position, Color.green, 0.1f);
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
