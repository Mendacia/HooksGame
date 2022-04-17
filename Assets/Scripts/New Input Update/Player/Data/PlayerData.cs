using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlaterData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Prefabs and Visuals")]
    public GameObject swingChainRoot = null;
    public GameObject swingChainLink = null;
    public GameObject spinAxis = null;
    public GameObject spinAnchor = null;
    public GameObject hookLineRenderer = null;

    [Header("Move State")]
    public float movementAcceleration = 25f;
    public float movementSpeedCap = 8f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;

    [Header("In Air State")]
    public float coyoteTimeLength = 0.1f;

    [Header("Hook Through State")]
    public float hookVelocity = 10f;

    [Header("Spin State")]
    public float orbitalAcceleration = 10f;
    public float spinSpeedThreshold = 10f;
    public float orbitalSpeedCap = 40f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
