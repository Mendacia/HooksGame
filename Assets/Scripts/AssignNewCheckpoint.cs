using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class AssignNewCheckpoint : MonoBehaviour
{
    public GameObject spawnLocation;
    public bool enableAnalyticsTracking = true;
    public bool enableStartProgressionTracking = true;
    public bool enableCompleteProgressionTracking = true;
    public string checkpointName;
    public string lastCheckpointName;
    private bool triggeredBefore;
    private void OnTriggerStay2D(Collider2D collision)
    {
        var PlayerControls = collision.gameObject.GetComponent<PlayerControlsNew>();
        if (PlayerControls != null)
        {
            PlayerControls.SetCheckPointToThis(spawnLocation.transform.position);
            if(triggeredBefore == false && enableAnalyticsTracking)
            {
                if (enableStartProgressionTracking)
                {
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, checkpointName);
                }
                if (enableCompleteProgressionTracking)
                {
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, lastCheckpointName);
                }
                triggeredBefore = true;
            }
        }
    }
}
