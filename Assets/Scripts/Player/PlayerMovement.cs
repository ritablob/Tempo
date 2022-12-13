using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] GameObject attackIndicator;
    [SerializeField] GameObject parryShield;

    [Header("Character Stats")]
    [SerializeField] int HP = 10;
    [SerializeField] float speed = 1f;
    [SerializeField] CharacterController charController;
    [SerializeField] float controllerDeadZone = 0.1f;

    [Header("Attacks")]
    [SerializeField] Animator anim;

    [Header("Other")]
    [SerializeField] RhythmKeeper rhythmKeeper;
    [SerializeField] Camera sceneCamera;

    public string lastBeat;

    private Vector2 movement;
    private Vector3 aim;
    private float hitStunRemaining = 0;
    private PlayerControls playerControls;
    private bool isAttacking; //Prevents the player from acting during an attack
    private bool isLaunching;
    private bool isParrying;
    private bool canParry = true;

    [SerializeField] private Vector3 offset;

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
        if(hitStunRemaining > 0)
        {
            hitStunRemaining -= Time.deltaTime;
            return;
        }

        if (!isAttacking && !isParrying)
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
            lastBeat = rhythmKeeper.timingKey; //Get timing of input
            return;
        }
        else if (playerControls.Player.Attack_2.IsPressed())
        {
            anim.SetTrigger("Heavy");
            lastBeat = rhythmKeeper.timingKey; //Get timing of input
            return;
        }
        else if (playerControls.Player.Parry.WasPressedThisFrame() && canParry)
        {
            isParrying = true;
            StartCoroutine(parryTiming());
            parryShield.SetActive(true);
            canParry = false;
        }
    }

    void HandleInput()
    {
        movement = playerControls.Player.Move.ReadValue<Vector2>();
        aim = playerControls.Player.Look.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        UpdateRotation();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        charController.Move(move  * Time.deltaTime * speed);
    }

    void HandleRotation()
    {
        if (Mathf.Abs(aim.x) > controllerDeadZone || Mathf.Abs(aim.y) > controllerDeadZone)
        {
            Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

            if(playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
            }
        }
    }

    void UpdateRotation()
    {
        /* - camera rotation on y
         * - player x + z offset calculation based on camera rotation
         * - change player movement based on offset 
         */

        //float cameraAngleY = Quaternion.Angle(sceneCamera.transform.rotation);
        float coefficient = sceneCamera.transform.rotation.y / 360.00f; // camera rotation coefficient (0.0-1.0)

        if (coefficient < 0.25f)
        {
            offset.x = -coefficient * 4.0f;
            offset.z = coefficient * 4.0f;
        }
        else if (coefficient < 0.5f)
        {
            offset.x = -coefficient * 4.0f;
            offset.z = -(coefficient - 0.25f) * 4.0f;
        }
        else if (coefficient < 0.75f)
        {
            offset.x = ((coefficient - 0.5f) * 4.0f) - 1.0f;
            offset.z = -(coefficient - 0.5f) * 4.0f;
        }
        else
        {
            offset.x = (coefficient - 0.75f) * 4.0f;
            offset.z = (coefficient - 0.75f)*4.0f -1.0f;
        }
        //Debug.Log($"camera up x - " + cameraVector.x + ", camera up y - " + cameraVector.z) ;
        Debug.LogError(sceneCamera.transform.rotation.y);
        Debug.LogWarning("offset = " + offset.x + ", " + offset.z);
    }


    //Taking Damage
    public void TakeDamage(int damage, float hitStun, float knockBack, Transform hitBoxTransform)
    {
        if (!isParrying)
        {
            anim.Play("Base Layer.Test_Idle");
            isLaunching = false;
            isAttacking = false;
            hitStunRemaining = hitStun;
            HP -= damage;
            Vector3 launchDir = gameObject.transform.position - hitBoxTransform.position;
            launchDir.y = 0;
            launchDir.Normalize();
            charController.Move(launchDir * knockBack);
        }
    }


    //Coroutines
    IEnumerator parryTiming()
    {
        yield return new WaitForSeconds(0.1f);
        isParrying = false;
        parryShield.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        canParry = true;
    }
}