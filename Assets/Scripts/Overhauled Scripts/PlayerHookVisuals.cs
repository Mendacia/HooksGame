using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHookVisuals : MonoBehaviour
{
    private LineRenderer chainLineRenderer;
    private SpriteRenderer chainEndSpriteRenderer;
    private Transform targetHook;
    [SerializeField] private Transform player;
    void Start()
    {
        chainLineRenderer = gameObject.GetComponent<LineRenderer>();
        chainEndSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void GiveTheHookVisualsScriptTheSelectedTarget(Transform recievedTarget)
    {
        targetHook = recievedTarget;
    }

    public void DrawHook()
    {
        chainEndSpriteRenderer.enabled = true;
        chainLineRenderer.enabled = true;
        chainLineRenderer.positionCount = 2;
        var positions = new List<Vector3>();
        positions.Add(new Vector2(targetHook.position.x, targetHook.position.y));
        positions.Add(player.position);
        chainLineRenderer.SetPositions(positions.ToArray());
    }

    public void KillHook()
    {
        chainLineRenderer.enabled = false;
        chainEndSpriteRenderer.enabled = false;
    }
}
