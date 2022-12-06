using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMovement : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody rb;
    private Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Keyboard.current.anyKey.isPressed)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                move = Vector3.forward; // 1,0
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                move = Vector3.back; // -1,0
            }
            else if (Keyboard.current.aKey.isPressed)
            {
                move = Vector3.left;
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                move = Vector3.right;
            }
            rb.AddForce(move * speed, ForceMode.VelocityChange);
        }

        {
            move = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }


    }
}
