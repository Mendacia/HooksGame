using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class AssignNewCheckpoint : MonoBehaviour
{
    public GameObject spawnLocation;
    public bool enableAnalyticsTracking = true;
    public string checkpointName;
    private bool triggeredBefore;
    private void OnTriggerStay2D(Collider2D collision)
    {
        var PlayerControls = collision.gameObject.GetComponent<PlayerControls>();
        if (PlayerControls != null)
        {
            PlayerControls.currentCheckpoint = spawnLocation.transform;
            if(triggeredBefore == false && enableAnalyticsTracking)
            {
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, checkpointName);
                triggeredBefore = true;
            }
        }
    }
}
