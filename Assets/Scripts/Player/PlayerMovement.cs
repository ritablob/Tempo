using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] GameObject parryShield;
    [SerializeField] GameObject shadowClone;
    [SerializeField] Transform shadowCloneSpawn;
    [SerializeField] Animator anim;
    [SerializeField] Material normal, hit;
    [SerializeField] Renderer modelRenderer;
    [SerializeField] int matIndex;

    [Header("Character Stats")]
    public float HP = 100;
    public bool doubleTime;
    [SerializeField] float speed = 1f;
    [SerializeField] CharacterController charController;
    [SerializeField] float controllerDeadZone = 0.1f;
    [SerializeField] Color playerColor;

    [Header("Other")]
    [SerializeField] RhythmKeeper rhythmKeeper;
    [SerializeField] AudioSource sfx;
    [SerializeField] float gamepadRumble1 = 0.5f;
    [SerializeField] float gamepadRumble2 = 0.5f;

    [HideInInspector]
    public int heldDown;
    public int playerIndex;
    public float lastBeatPercentage;
    public float maxValidInputTime; //Used to see if the next move falls under the correct combo timing
    public float validInputTimer; //Tracks the elapsed time of the current beat

    private Vector2 movement;
    private Vector3 aim;
    private Vector3 launchDirection;
    private int ultimateCharge;
    private float hitStunRemaining = 0;
    private Camera sceneCamera;
    private PlayerControls playerControls;
    private PlayerInput playerInput;
    public bool isGamepad;
    private bool takeKnockBack;
    private bool isAttacking; //Prevents the player from acting during an attack
    private bool canMove;
    private bool isLaunching;
    private bool isParrying;
    private bool canParry = true;
    private bool canDodge = true;
    private StatusEffects statusEffects;

    private Vector3 offset;
    //private bool hasOffset = false;
    //private bool isNotMoving; // we update the rotation of movement when the character has stopped moving

    void Awake()
    {
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        charController = GetComponent<CharacterController>();
        rhythmKeeper = GameObject.FindObjectOfType<RhythmKeeper>();
        sceneCamera = GameObject.FindObjectOfType<Camera>();
        statusEffects = GetComponent<StatusEffects>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    //Input system related functions
    public void LeftShoulder(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { heldDown++; }
        if (ctx.canceled) { heldDown--; }
        if (heldDown == 2 && ultimateCharge >= 50) { Debug.Log("ULTIMATE!"); ultimateCharge = 0; }
    }
    public void RightShoulder(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { heldDown++; }
        if (ctx.canceled) { heldDown--; }
        if (heldDown == 2 && ultimateCharge >= 50) { Debug.Log("ULTIMATE!"); ultimateCharge = 0; }
    }
    public void ControllerType(PlayerInput _playerInput)
    {
        isGamepad = _playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
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
        if (!isAttacking && !isParrying && canDodge && ctx.performed) //If not attacking, do attack logic
        {
            anim.SetTrigger("Attack_1");
            StartAttack();

            //SoundPlayer.PlaySound(playerIndex, "deal_damage");
            if (maxValidInputTime == 0) //Get timing of input if not in combo
            {
                lastBeatPercentage = rhythmKeeper.validInputTimer / rhythmKeeper.maxValidInputTime;
                if (doubleTime && lastBeatPercentage < 0.5f) lastBeatPercentage *= 2;
            }
            else //If in combo, calculate based on internal rhythm tracking
            { 
                lastBeatPercentage = validInputTimer / maxValidInputTime;
                if (doubleTime && lastBeatPercentage < 0.5f) lastBeatPercentage *= 2;
            }
            //SoundPlayer.PlaySound("glint");
        }
    }
    public void AttackHeavy(InputAction.CallbackContext ctx)
    {
        if (!isAttacking && !isParrying && canDodge && ctx.performed) //If not attacking, do attack logic
        {
            anim.SetTrigger("Attack_2");
            StartAttack();

            //SoundPlayer.PlaySound(playerIndex, "deal_damage");
            if (maxValidInputTime == 0) //Get timing of input if not in combo
            {
                lastBeatPercentage = rhythmKeeper.validInputTimer / rhythmKeeper.maxValidInputTime;
                if (doubleTime && lastBeatPercentage < 0.5f) lastBeatPercentage *= 2;
            }
            else 
            { 
                lastBeatPercentage = validInputTimer / maxValidInputTime;
                if (doubleTime && lastBeatPercentage < 0.5f) lastBeatPercentage *= 2;
            }
        }
    }
    public void Special(InputAction.CallbackContext ctx)
    {
        if (!isAttacking && !isParrying && canDodge && ctx.performed) //If not attacking, do attack logic
        {
            anim.SetTrigger("Special");
        }
    }
    public void Dodge(InputAction.CallbackContext ctx)
    {
        if (canDodge && ctx.performed && anim.GetBool("Running") && !isAttacking)
        {
            MiscLayer();
            anim.SetTrigger("Dodge");
            //SoundPlayer.PlaySound(playerIndex, "dodge");
            StartCoroutine(dodgeTiming());
        }
        else if (canParry && ctx.performed && !anim.GetBool("Running") && !isAttacking)
        {
            //SoundPlayer.PlaySound(playerIndex, "parry");
            MiscLayer();
            anim.SetTrigger("Parry");
            StartCoroutine(parryTiming());
        }
    }
    //Input related functions end


    //Combat-related functions
    public void StartAttack() { isAttacking = true; canMove = false; validInputTimer = 0; maxValidInputTime = 0; AttackLayer(); }
    public void CanCancelAttack() { isAttacking = false; isLaunching = false; canMove = false; }
    public void EndAttack() 
    {
        isAttacking = false; 
        isLaunching = false; 
        canMove = false; 
        validInputTimer = 0; 
        maxValidInputTime = 0; 
        anim.ResetTrigger("Light"); 
        anim.ResetTrigger("Heavy");
        ResetLayers();
        aim.x = 0; aim.y = 0; 
    }
    public void CanMove() { canMove = true; }
    public void SnapToOpponent()
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
    public void ResetLayers() { anim.SetLayerWeight(0, 1); anim.SetLayerWeight(1, 0); anim.SetLayerWeight(2, 0); }
    public void AttackLayer() { anim.SetLayerWeight(0, 0); anim.SetLayerWeight(1, 1); anim.SetLayerWeight(2, 0); }
    public void MiscLayer() { anim.SetLayerWeight(0, 0); anim.SetLayerWeight(1, 0); anim.SetLayerWeight(2, 1); }
    public void LaunchPlayer(float units)
    {
        isLaunching = transform;
        launchDirection = gameObject.transform.forward * -units;
    }
    public void EndLaunch()
    {
        isLaunching = false;
    }
    public void BeatsForNextAttack(int numOfBeats) //Use eighth notes for calculations
    {
        maxValidInputTime = rhythmKeeper.beatLength / 2; //Get time of eighth notes
        maxValidInputTime *= numOfBeats; //Set maxValidInputTime to x eighth notes
        validInputTimer = 0;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void AddUltimateCharge(int amnt)
    {
        ultimateCharge += amnt;
        Debug.Log(ultimateCharge);
    }
    //Combat-related functions end



    //Update Function
    void Update()
    {
        if (hitStunRemaining > 0) //If in hitstun, skip rest of update
        {
            hitStunRemaining -= Time.deltaTime;
            Material[] mats = modelRenderer.materials;
            mats[matIndex] = hit;
            modelRenderer.materials = mats;
            if (takeKnockBack) { charController.Move(launchDirection * Time.deltaTime); } //Launch player over time if they are being knocked back
            return;
        }
        else if (modelRenderer.material != normal) 
        {
            Material[] mats = modelRenderer.materials;
            mats[matIndex] = normal;
            modelRenderer.materials = mats;
        }

        if (maxValidInputTime != 0) { validInputTimer += Time.deltaTime; } //Add time to the combo counter

        if (!isAttacking && !isParrying) //If the player is not attacking or parrying, run general movement check
        {
            HandleMovement();
            HandleRotation();
        }
        else if (canMove && isAttacking) //If the player is attacking but can move during the attack, do a movement checl
        {
            HandleMovement();
        }
        else if (isLaunching) //If the player needs to be launched, move them
        {
            charController.Move(launchDirection * Time.deltaTime);
        }
    }
    //End of update


    void HandleMovement()
    {
        Vector3 move = Quaternion.Euler(0, sceneCamera.transform.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y);
        move.y = -2;
        charController.Move(move * Time.deltaTime *speed);
        if(move.x == 0 && move.z == 0) { anim.SetBool("Running", false); }
        else { anim.SetBool("Running", true); }
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


    //Taking Damage & Status Effects
    public void TakeDamage(float damage, float hitStun, float knockBack, Transform hitBoxTransform)
    {
        if (!isParrying)
        {
            takeKnockBack = true;
            StartCoroutine(TakeKnockBack());
            anim.SetTrigger("Hit");
            MiscLayer();
            isLaunching = false;
            isAttacking = false;
            hitStunRemaining = hitStun;
            HP -= damage * statusEffects.exposeStacks;
            Material[] mats = modelRenderer.materials;
            mats[matIndex] = hit;
            modelRenderer.materials = mats;
            Vector3 launchDir = gameObject.transform.position - hitBoxTransform.position;
            launchDir.y = 0;
            launchDir.Normalize(); //knockback determines intensity of launch
            launchDirection = launchDir * knockBack;
            if (isGamepad)
            {
                StartCoroutine(Rumble());
            }
        }
        else
        {
            canParry = true;
            parryShield.SetActive(false);
        }
    }



    //Coroutines
    IEnumerator parryTiming()
    {
        isParrying = true;
        anim.SetTrigger("Parry");
        parryShield.SetActive(true);
        canParry = false;
        yield return new WaitForSeconds(0.2f);
        isParrying = false;
        parryShield.SetActive(false);
        SoundPlayer.StopSound("parry");
        yield return new WaitForSeconds(0.7f);
        canMove = true;
        isAttacking = false;
        canParry = true;
    }
    IEnumerator dodgeTiming()
    {
        canDodge = false;
        anim.SetTrigger("Dodge");
        yield return new WaitForSeconds(1.75f);
        canDodge = true;
    }
    IEnumerator TakeKnockBack()
    {
        isLaunching = true;
        yield return new WaitForSeconds(0.1f);
        takeKnockBack = false;
        isLaunching = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    IEnumerator Rumble()
    {
        Gamepad.current.SetMotorSpeeds(gamepadRumble1, gamepadRumble2);
        yield return new WaitForSeconds(0.3f);
        Gamepad.current.SetMotorSpeeds(0f,0f);
    }
}