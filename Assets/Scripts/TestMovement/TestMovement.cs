using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    [SerializeField] TextMeshProUGUI text;

    public string lastBeat;

    private Vector2 movement;
    private Vector3 aim;
    private float hitStunRemaining = 0;
    private float maxValidInputTime; //Used to see if the next move falls under the correct combo timing
    private float validInputTimer; //Tracks the elapsed time of the current beat
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

        if(maxValidInputTime != 0) { validInputTimer += Time.deltaTime; }

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

    //Combat-related functions
    public void StartAttack() { isAttacking = true; canMove = false; validInputTimer = 0; maxValidInputTime = 0; }
    public void CanCancelAttack() { isAttacking = false; isLaunching = false; canMove = false; }
    public void EndAttack() { isAttacking = false; isLaunching = false; canMove = false; validInputTimer = 0; maxValidInputTime = 0; }
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
    public void BeatsForNextAttack(int numOfBeats) //Use eighth notes for calculations
    {
        maxValidInputTime = rhythmKeeper.beatLength / 2; //Get time of eighth notes
        maxValidInputTime *= numOfBeats; //Set maxValidInputTime to x eighth notes
        rhythmKeeper.SpawnArrow(maxValidInputTime);
    }


    void HandleAction()
    {
        float beatPerc = validInputTimer / maxValidInputTime * 100; //Calculate percentage of beat

        if (playerControls.Player.Attack_1.IsPressed())
        {
            if (rhythmKeeper.timingKey == "Miss" && maxValidInputTime == 0) { anim.SetTrigger("Missed"); return; }

            anim.SetTrigger("Light");

            if(maxValidInputTime == 0) { lastBeat = rhythmKeeper.timingKey; } //Get timing of input
            if (beatPerc < rhythmKeeper.normalLeewayPerc) { anim.SetTrigger("Missed"); }
            else if(beatPerc >= rhythmKeeper.normalLeewayPerc && beatPerc < rhythmKeeper.perfectLeewayPerc) { lastBeat = "Early"; }
            else if(beatPerc >= rhythmKeeper.perfectLeewayPerc && beatPerc < 100) { lastBeat = "Perfect"; }
            else { lastBeat = "Early"; }

            text.text = $"{beatPerc} / {lastBeat}";
            return;
        }
        else if (playerControls.Player.Attack_2.IsPressed())
        {
            if (rhythmKeeper.timingKey == "Miss" && maxValidInputTime == 0) { anim.SetTrigger("Missed"); return; }

            anim.SetTrigger("Heavy");

            if (maxValidInputTime == 0) { lastBeat = rhythmKeeper.timingKey; } //Get timing of input

            if (beatPerc < rhythmKeeper.normalLeewayPerc) { anim.SetTrigger("Missed"); }
            else if (beatPerc >= rhythmKeeper.normalLeewayPerc && beatPerc < rhythmKeeper.perfectLeewayPerc) { lastBeat = "Early"; }
            else if (beatPerc >= rhythmKeeper.perfectLeewayPerc && beatPerc < 100) { lastBeat = "Perfect"; }
            else { lastBeat = "Early"; }

            text.text = $"{beatPerc} / {lastBeat}";
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
                Quaternion newRotation = Quaternion.LookRotation(-playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
            }
        }
        else if(movement.x != 0 && movement.y != 0 && aim.x < controllerDeadZone && aim.y < controllerDeadZone)
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
        yield return new WaitForSeconds(0.9f);
        canParry = true;
    }
}
