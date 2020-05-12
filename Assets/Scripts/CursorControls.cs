using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControls : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector2 direction;
    public GameObject cursor;
    public GameObject aimBot;
    public Transform[] hookTargets;
    private Transform selectedTarget;

    private void Update()
    {
        //Makes a mouse cursor out of a game object
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3 (mousePosition.x, mousePosition.y, 0);

        //Selects the closest hook location
        selectedTarget = GetClosestHook(hookTargets);
        aimBot.transform.position = new Vector3(selectedTarget.position.x, selectedTarget.position.y, 0);
     
    }





    //Code that actually runs when detecting the closest hook location
    Transform GetClosestHook(Transform[] target)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = cursor.transform.position;
        foreach (Transform potentialTarget in target)
        {
            Vector2 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

}
