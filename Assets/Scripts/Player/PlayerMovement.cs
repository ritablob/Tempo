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
    [SerializeField] GameObject parryShield;
    [SerializeField] GameObject shadowClone;
    [SerializeField] Animator anim;

    [Header("Character Stats")]
    public float HP = 10;
    [SerializeField] float speed = 1f;
    [SerializeField] CharacterController charController;
    [SerializeField] float controllerDeadZone = 0.1f;

    [Header("Other")]
    [SerializeField] RhythmKeeper rhythmKeeper;

    //[HideInInspector]
    public string lastBeat;

    private Vector2 movement;
    private Vector3 aim;
    private float hitStunRemaining = 0;
    private float maxValidInputTime; //Used to see if the next move falls under the correct combo timing
    private float validInputTimer; //Tracks the elapsed time of the current beat
    private PlayerControls playerControls;
    private Camera sceneCamera;
    private bool isGamepad; 
    private bool isAttacking; //Prevents the player from acting during an attack
    private bool isDodging;
    private bool canMove;
    private bool isLaunching;
    private bool isParrying;
    private bool canParry = true;
    private bool canDodge = true;

    private Vector3 offset;
    private bool hasOffset = false;
    //private bool isNotMoving; // we update the rotation of movement when the character has stopped moving

    void Awake()
    {
        playerControls = new PlayerControls();
        charController = GetComponent<CharacterController>();
        rhythmKeeper = GameObject.FindObjectOfType<RhythmKeeper>();
        sceneCamera = GameObject.FindObjectOfType<Camera>();
    }

    //Input system related functions
    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext ctx)
    {
        aim = ctx.ReadValue<Vector2>();
    }
    public void AttackLight(InputAction.CallbackContext ctx)
    {
        if (!isAttacking && !isDodging) //If not attacking, do attack logic
        {
            float beatPerc = validInputTimer / maxValidInputTime * 100; //Calculate percentage of beat

            if (rhythmKeeper.timingKey == "Miss" && maxValidInputTime == 0) { anim.SetTrigger("Missed"); return; }

            anim.SetTrigger("Light");

            if (maxValidInputTime == 0) { lastBeat = rhythmKeeper.timingKey; return; } //Get timing of input
            if (beatPerc < rhythmKeeper.normalLeewayPerc) { anim.SetTrigger("Missed"); }
            else if (beatPerc >= rhythmKeeper.normalLeewayPerc && beatPerc < rhythmKeeper.perfectLeewayPerc) { lastBeat = "Early"; }
            else if (beatPerc >= rhythmKeeper.perfectLeewayPerc && beatPerc < 100) { lastBeat = "Perfect"; }
            else { anim.SetTrigger("Missed"); }

            return;
        }
    }
    public void AttackHeavy(InputAction.CallbackContext ctx)
    {
        if (!isAttacking && !isDodging) //If not attacking, do attack logic
        {
            float beatPerc = validInputTimer / maxValidInputTime * 100; //Calculate percentage of beat

            if (rhythmKeeper.timingKey == "Miss" && maxValidInputTime == 0) { anim.SetTrigger("Missed"); return; }

            anim.SetTrigger("Heavy");

            if (maxValidInputTime == 0) { lastBeat = rhythmKeeper.timingKey; return; } //Get timing of input
            if (beatPerc < rhythmKeeper.normalLeewayPerc) { anim.SetTrigger("Missed"); }
            else if (beatPerc >= rhythmKeeper.normalLeewayPerc && beatPerc < rhythmKeeper.perfectLeewayPerc) { lastBeat = "Early"; }
            else if (beatPerc >= rhythmKeeper.perfectLeewayPerc && beatPerc < 100) { lastBeat = "Perfect"; }
            else { anim.SetTrigger("Missed"); }

            return;
        }
    }
    public void Dodge(InputAction.CallbackContext ctx)
    {
        if (canDodge && ctx.performed)
        {
            anim.SetTrigger("Dodge");
            EndAttack();
            StartCoroutine(dodgeTiming());
        }
    }
    public void Parry(InputAction.CallbackContext ctx)
    {
        if (canParry)
        {
            StartCoroutine(parryTiming());
        }
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    //Input related functions end


    //Combat-related functions
    public void StartAttack() { isAttacking = true; canMove = false; validInputTimer = 0; maxValidInputTime = 0; }
    public void CanCancelAttack() { isAttacking = false; isLaunching = false; canMove = false; }
    public void EndAttack() { isAttacking = false; isLaunching = false; canMove = false; validInputTimer = 0; maxValidInputTime = 0; aim = new Vector2(0, 0); anim.StopPlayback(); }
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
    public void SpawnShadowClone(float _fadeSpeed)
    {
        GameObject _shadowClone = Instantiate(shadowClone, transform.position, transform.rotation);
        _shadowClone.AddComponent<FadeObject>();
        _shadowClone.GetComponent<FadeObject>().fadeSpeed = _fadeSpeed;
    }
    //Combat-related functions end


    void Update()
    {
        if (hitStunRemaining > 0) //If in hitstun, skip rest of update
        {
            hitStunRemaining -= Time.deltaTime;
            return;
        }

        if (maxValidInputTime != 0) { validInputTimer += Time.deltaTime; } //Add time to the combo counter

        if (!isAttacking && !isParrying) //If the player is not attacking or parrying, run general movement check
        {
            HandleMovement();
            HandleRotation();
            return;
        }
        else if (canMove) //If the player is attacking but can move during the attack, do a movement checl
        {
            HandleMovement();
        }
        else if (isLaunching) //If the player needs to be launched, move them
        {
            charController.Move(aim * Time.deltaTime);
        }
    }

    void HandleMovement()
    {
        Vector3 move = Quaternion.Euler(0, sceneCamera.transform.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y);
        charController.Move(move  * Time.deltaTime * speed);
    }

    void HandleRotation()
    {
        if (isGamepad)
        {
            if (Mathf.Abs(aim.x) > controllerDeadZone || Mathf.Abs(aim.y) > controllerDeadZone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newRotation = Quaternion.Euler(0, sceneCamera.transform.eulerAngles.y, 0) * Quaternion.LookRotation(-playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
                }
            }
            else if (movement.x != 0 || movement.y != 0)
            {
                Vector3 newMovement = Quaternion.Euler(0, sceneCamera.transform.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y);
                Quaternion newRotation = Quaternion.LookRotation(-newMovement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
            }
        }
        else
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj != this.gameObject)
                {
                    Vector3 away = gameObject.transform.position - obj.transform.position;
                    Quaternion awayRot = Quaternion.LookRotation(away);
                    transform.rotation = awayRot;
                }
            }
        }
    }


    //Taking Damage
    public void TakeDamage(float damage, float hitStun, float knockBack, Transform hitBoxTransform)
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
        isParrying = true;
        parryShield.SetActive(true);
        canParry = false;
        yield return new WaitForSeconds(0.2f);
        isParrying = false;
        parryShield.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        canParry = true;
    }
    IEnumerator dodgeTiming()
    {
        speed *= 4;
        canDodge = false;
        isDodging = true;
        yield return new WaitForSeconds(0.33f);
        speed /= 4;
        isDodging = false;
        yield return new WaitForSeconds(1.17f);
        canDodge = true;
    }
}