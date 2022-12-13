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
    [SerializeField] Transform rayCastStart;

    public string lastBeat;

    private Vector2 movement;
    private Vector3 aim;
    private float hitStunRemaining = 0;
    private PlayerControls playerControls;
    private bool isAttacking; //Prevents the player from acting during an attack
    private bool canMove;
    private bool isLaunching;
    private bool isParrying;
    private bool canParry = true;

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
        else if (canMove)
        {
            HandleInput();
            HandleMovement();
        }
    }

    public void StartAttack() { isAttacking = true; canMove = false; }
    public void EndAttack() { isAttacking = false; isLaunching = false; canMove = false; }
    public void CanMove() { canMove = true; }
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
            }
        }
        else if(movement.x != 0 && movement.y != 0)
        { 
            Vector3 newMovement = new Vector3(movement.x, 0, movement.y);
            Quaternion newRotation = Quaternion.LookRotation(-newMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
        }
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
