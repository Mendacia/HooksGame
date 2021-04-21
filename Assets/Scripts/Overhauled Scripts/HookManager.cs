using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookManager : MonoBehaviour
{
    public List<Transform> hooks;
    [Header("Set these to their corresponding objects")]
    [SerializeField] private Transform player = null;
    private float realPlayerTargetingRadius;
    private float cursorTargetingRadius;
    [SerializeField] private LayerMask hookFinder = 0;
    [System.NonSerialized] public Vector3 cursorLocation = Vector3.zero;
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            hooks.Add(child);
        }
    }

    public void updateCursorLocationInHookManagerScript(Vector3 location)
    {
        cursorLocation = location;
    }

    public void TellHookManagerWhatTheTargetingRadiiAre(float real, float cursor)
    {
        realPlayerTargetingRadius = real;
        cursorTargetingRadius = cursor;
    }

    public Transform GetClosestHook(Transform[] targets)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = cursorTargetingRadius;
        foreach (Transform potentialTarget in targets)
        {
            var hookAndWallDetectionRaycast = Physics2D.Raycast(player.transform.position, potentialTarget.transform.position - player.transform.position, realPlayerTargetingRadius, hookFinder);
            Debug.DrawRay(player.transform.position, potentialTarget.transform.position - player.transform.position);
            if (hookAndWallDetectionRaycast.collider == null)
            {
                //Missed
                continue;
            }
            if (hookAndWallDetectionRaycast.collider.gameObject.tag == "Wall" || hookAndWallDetectionRaycast.collider.gameObject.tag == "Ground")
            {
                //Hit Wall
                continue;
            }
            if (hookAndWallDetectionRaycast.collider.gameObject.transform == potentialTarget)
            {
                Vector2 directionToTarget = potentialTarget.position - cursorLocation;
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
