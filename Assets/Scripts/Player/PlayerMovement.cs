using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    private float movementX;
    private float movementY;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 move = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(move * movementSpeed);
    }


    private void OnMove(InputValue movementValue)
    {
        if (movementValue.isPressed)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();

            movementX = movementVector.x;
            movementY = movementVector.y;
        }
        else
        {
            movementX = 0;
            movementY = 0;
        }


    }
}
