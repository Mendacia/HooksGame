using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public KeyCode left, right, jump;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Basic Controls for Left and Right
        if (Input.GetKey(right))
        {
            rb.velocity=new Vector2(5,0);
        }

        if (Input.GetKey(left))
        {
            rb.velocity=new Vector2(-5,0);
        }


        //Basic Jump Controls


        //Hook Controls
    }
}
