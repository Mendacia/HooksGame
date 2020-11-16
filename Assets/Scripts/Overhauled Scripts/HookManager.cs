using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookManager : MonoBehaviour
{
    public List<Transform> hooks;
    [Header("Set these to their corresponding objects")]
    [SerializeField] private Transform player;
    private float targetingRadius;
    private float realTargetingRadius;
    [SerializeField] private LayerMask hookFinder;
    [SerializeField] private Vector3 cursorLocation = Vector3.zero;
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            hooks.Add(child);
        }
    }

    public void TellHookManagerWhatTheTargetingRadiusAre(float temp, float real)
    {
        targetingRadius = temp;
        realTargetingRadius = real;
    }

    public Transform GetClosestHook(Transform[] targets)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = targetingRadius;
        foreach (Transform potentialTarget in targets)
        {
            var hookAndWallDetectionRaycast = Physics2D.Raycast(player.transform.position, potentialTarget.transform.position - player.transform.position, realTargetingRadius, hookFinder);

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
            if (hookAndWallDetectionRaycast.collider.gameObject.tag == "Hook")
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
