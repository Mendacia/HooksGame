using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlaterData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementAcceleration = 25f;
    public float movementSpeedCap = 8f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;

    [Header("In Air State")]
    public float coyoteTimeLength = 0.1f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
