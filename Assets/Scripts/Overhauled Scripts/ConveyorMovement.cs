using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovement : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    [SerializeField] private float speed;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var pos = myRigidbody.position;

        myRigidbody.position += Vector2.left * speed * Time.fixedDeltaTime;

        myRigidbody.MovePosition(pos);
    }
}
