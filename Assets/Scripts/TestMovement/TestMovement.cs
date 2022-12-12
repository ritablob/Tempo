using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TestMovement : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] GameObject attackIndicator;

    [Header("Character Stats")]
    [SerializeField] float speed = 1f;
    [SerializeField] CharacterController charController;
    [SerializeField] float controllerDeadZone = 0.1f;
    [SerializeField] float gamePadSmoothingRate = 1000f;

    [Header("Attacks")]
    [SerializeField] Animator anim;
    [SerializeField] Animation[] attackList;

    private Vector2 movement;
    private Vector3 aim;
    private PlayerControls playerControls;
    private bool isAttacking; //Prevents the player from acting during an attack
    private bool isLaunching;

    void Awake()
    {
        playerControls = new PlayerControls();
        charController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        if (!isAttacking)
        {
            HandleInput();
            HandleAction();
            HandleMovement();
            HandleRotation();
        }
        else if (isLaunching)
        {
            charController.Move(aim * Time.deltaTime);
        }
    }

    public void StartAttack() { isAttacking = true; }
    public void EndAttack() { isAttacking = false; isLaunching = false; }
    public void LaunchPlayer(float units)
    {
        isLaunching = transform;
        Vector3 launchDirection = gameObject.transform.forward * -units;
        aim = launchDirection;
    }
    public void EndLaunch()
    {
        isLaunching = false;
    }

    void HandleAction()
    {
        if (playerControls.Player.Attack_1.IsPressed())
        {
            anim.SetTrigger("Light");
            return;
        }
        else if (playerControls.Player.Attack_2.IsPressed())
        {
            anim.SetTrigger("Heavy");
            return;
        }
    }

    void HandleInput()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        aim = playerControls.Player.Look.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        charController.Move(move * Time.deltaTime * speed);
    }

    void HandleRotation()
    {
        if (Mathf.Abs(aim.x) > controllerDeadZone || Mathf.Abs(aim.y) > controllerDeadZone)
        {
            Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

            if(playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, gamePadSmoothingRate * Time.deltaTime);
            }
        }
    }
}
