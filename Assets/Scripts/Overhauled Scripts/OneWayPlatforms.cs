using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatforms : MonoBehaviour
{
    private CompositeCollider2D myCollider;
    private void Start()
    {
        myCollider = gameObject.GetComponent<CompositeCollider2D>();
    }
    private void Update()
    {
    }
}
