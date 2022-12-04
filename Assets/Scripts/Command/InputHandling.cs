using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Movement input (wasd) - handled through the Unity Input System (see PlayerMovement script).
/// Everything else - handled below.
/// </summary>
public class InputHandling : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }
    void HandleInput()
    {
    }
}
