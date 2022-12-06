using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    private Vector2 move2;
    private Vector3 move3;
    private Rigidbody rb;
    private bool buttonPressed;
    public PlayerInput playerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (buttonPressed)
        {
            Move();
            move3 = new Vector3(move2.x, 0, move2.y);
            rb.AddForce(move3 * movementSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        move2 = playerInput.actions["Move"].ReadValue<Vector2Int>();
    }
    /*private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); // kodėl???
        //buttonPressed = movementValue.isPressed;
        movementX = movementVector.x;
        movementY = movementVector.y;
    }*/

}
