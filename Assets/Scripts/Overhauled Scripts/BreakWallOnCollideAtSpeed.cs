using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWallOnCollideAtSpeed : MonoBehaviour
{
    [SerializeField] private float magnitudeToBreakAt;
    [SerializeField] private GameObject breakageParticles;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.x > magnitudeToBreakAt)
        {
            Instantiate(breakageParticles);
            Destroy(gameObject);
        }
    }
}
