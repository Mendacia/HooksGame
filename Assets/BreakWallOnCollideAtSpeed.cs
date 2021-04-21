using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWallOnCollideAtSpeed : MonoBehaviour
{
    [SerializeField] private float magnitudeToBreakAt;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.x > magnitudeToBreakAt)
        {
            Destroy(gameObject);
        }
    }
}
