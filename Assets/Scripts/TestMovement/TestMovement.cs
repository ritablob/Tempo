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

    private Vector2 movement;
    private Vector3 aim;
    private float hitStunRemaining = 0;
    private PlayerControls playerControls;
    private bool isAttacking; //Prevents the player from acting during an attack
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
    }


    //Taking Damage
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if (!isParrying)
            {
                TakeDamage(other.GetComponent<Damage>().damage, other.GetComponent<Damage>().hitStun, other.GetComponent<Damage>().knockBack, other.transform);
            }
        }
    }
    void TakeDamage(int damage, float hitStun, float knockBack, Transform hitBoxTransform)
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
