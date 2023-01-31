using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Vars
    #region
    [Header("Visuals")]
    [SerializeField] GameObject shadowClone;
    [SerializeField] Transform shadowCloneSpawn;
    [SerializeField] Animator anim;
    [SerializeField] Material normal, hit;
    public Renderer modelRenderer;
    [SerializeField] int matIndex;
    [SerializeField] VisualEffect hitParticle;
    [SerializeField] GameObject particle1;
    [SerializeField] GameObject particle2;

    [Header("Character Stats")]
    public bool isPoleDancer = false;
    public int specialCharges;
    public int ultimateLimit;
    public float HP = 100;
    [SerializeField] float speed = 1f;
    [SerializeField] CharacterController charController;
    [SerializeField] Color playerColor;

    [Header("Other")]
    public PlayerInput playerInput;
    [SerializeField] RhythmKeeper rhythmKeeper;
    [SerializeField] AudioSource sfx;
    [SerializeField] EventCommunicator eventCommunicator;
    HitCanvasManager hitCanvasManager;
    PlayerIngameMenu inGameMenu;

    [HideInInspector]
    public int heldDown;
    public int playerIndex;
    public string lastBeatTiming;
    public float lastBeatTimingPerc;
    public int ultimateCharge;
    public bool longCombo;

    //States/local vars
    private Vector2 movement;
    private Vector3 launchDirection;
    private Camera sceneCamera;
    private PlayerControls playerControls;
    private StatusEffects statusEffects;
    private string triggerName;
    private float hitStunRemaining = 0;
    private float comboTimer;
    private float ourDeadTime;
    private bool ultChargePlayed;
    private bool isGamepad;
    private bool takeKnockBack;
    private bool isAttacking; //Prevents the player from acting during an attack
    private bool canMove = true;
    private bool isLaunching;
    private bool canDodge = true;
    private bool blendLayers;
    private WinManager win;
    #endregion

    //Start functions
    #region
    void Awake()
    {
        win = FindObjectOfType<WinManager>();
        EventSystem.Instance.AddEventListener("DeadTime", SetOurDeadTime);
        EventSystem.Instance.AddEventListener("New Beat", NewBeat);
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        charController = GetComponent<CharacterController>();
        rhythmKeeper = GameObject.FindObjectOfType<RhythmKeeper>();
        sceneCamera = GameObject.FindObjectOfType<Camera>();
        statusEffects = GetComponent<StatusEffects>();
        hitCanvasManager = FindObjectOfType<HitCanvasManager>();
        inGameMenu = GetComponent<PlayerIngameMenu>();
    }
    private void OnEnable()
    {
        playerControls.Enable();
        inGameMenu.playerMap = playerInput.currentActionMap;
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion

    //Input system related functions
    #region
    public void LeftShoulder(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { heldDown++; }
        if (ctx.canceled) { heldDown--; }
        if (heldDown == 2 && ultimateCharge >= ultimateLimit && ourDeadTime < 0 && canDodge && ctx.performed && rhythmKeeper.beatTiming != "DeadZone") 
        {
            particle1.SetActive(true); particle2.SetActive(true);
            anim.SetTrigger("ULTIMATE");
            ultChargePlayed = false;
            ultimateCharge = 0;
            StartAttack(false);
        }
    }
    public void RightShoulder(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) { heldDown++; }
        if (ctx.canceled) { heldDown--; }
        if (heldDown == 2 && ultimateCharge >= ultimateLimit && ourDeadTime < 0 && canDodge && ctx.performed && rhythmKeeper.beatTiming != "DeadZone")
        {
            particle1.SetActive(true); particle2.SetActive(true);
            anim.SetTrigger("ULTIMATE");
            ultChargePlayed = false;
            ultimateCharge = 0;
            StartAttack(false);
        }
    }
    public void ControllerType(PlayerInput _playerInput)
    {
        isGamepad = _playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }
    public void Attack_1(InputAction.CallbackContext ctx)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsTag("Exit")) { return; }

        if (ourDeadTime < 0 && canDodge && ctx.performed && rhythmKeeper.beatTiming != "DeadZone") //If not attacking, do attack logic
        {
            longCombo = false;
            anim.SetTrigger("Attack_1");
            StartAttack(false);
            return;
        }

        if (ourDeadTime < 0 && canDodge && ctx.canceled && rhythmKeeper.beatTiming != "DeadZone" && isAttacking && !anim.GetBool("Attack_1") && comboTimer < 0)
        {
            longCombo = false;
            anim.SetTrigger("Attack_1_Released");
            StartAttack(false);
            return;
        }

        if (ourDeadTime > 0 && canDodge && ctx.performed && rhythmKeeper.beatTiming != "DeadZone" && triggerName == null) //If attacking, read and remember input timing for attack buffer
        {
            triggerName = "Attack_1";
            lastBeatTimingPerc = Mathf.Abs(rhythmKeeper.validInputTimer); //Get absolute difference value
            lastBeatTiming = rhythmKeeper.beatTiming;
        }
    }
    public void Attack_2(InputAction.CallbackContext ctx)
    {
        if (anim.GetCurrentAnimatorStateInfo(1).IsTag("Exit")) { return; }

        if (ourDeadTime < 0 && canDodge && ctx.performed && rhythmKeeper.beatTiming != "DeadZone") //If not attacking, do attack logic
        {
            if (isPoleDancer) { SoundPlayer.PlaySound(1, "Pole_Swing"); }
            longCombo = false;
            anim.SetTrigger("Attack_2");
            StartAttack(false);
            return;
        }

        if (ourDeadTime < 0 && canDodge && ctx.canceled && rhythmKeeper.beatTiming != "DeadZone" && isAttacking && !anim.GetBool("Attack_2") && comboTimer < 0)
        {
            if (isPoleDancer) { SoundPlayer.PlaySound(1, "Pole_Swing"); }
            longCombo = false;
            anim.SetTrigger("Attack_2_Released");
            StartAttack(false);
            return;
        }

        if (ourDeadTime > 0 && canDodge && ctx.performed && rhythmKeeper.beatTiming != "DeadZone" && triggerName == null) //If attacking, read and remember input timing for attack buffer
        {
            triggerName = "Attack_2";
            lastBeatTimingPerc = Mathf.Abs(rhythmKeeper.validInputTimer); //Get absolute difference value
            lastBeatTiming = rhythmKeeper.beatTiming;
        }
    }
    public void Special(InputAction.CallbackContext ctx)
    {
        if (!isAttacking && canDodge && ctx.performed && specialCharges > 0) //If not attacking, do attack logic
        {
            specialCharges--;
            StartAttack(false);
            anim.SetTrigger("Special");
            lastBeatTimingPerc = Mathf.Abs(rhythmKeeper.validInputTimer); //Get absolute difference value
            lastBeatTiming = rhythmKeeper.beatTiming; //Get absolute difference value
        }
    }
    public void Dodge(InputAction.CallbackContext ctx)
    {
        if (canDodge && ctx.performed && anim.GetBool("Running") && !isAttacking && canMove)
        {
            MiscLayer();
            anim.SetTrigger("Dodge");
            StartCoroutine(dodgeTiming());
            return;
        }
    }
    #endregion
    //Input related functions end

    //Combat-related functions
    #region
    public void StartAttack(bool wasBuffered)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player != this.gameObject && Vector3.Distance(gameObject.transform.position, player.transform.position) < 1) //Auto-snap to opponent if close
            {
                Debug.Log(Vector3.Distance(gameObject.transform.position, player.transform.position) + "DISTANCE");
                SnapToOpponent();
            }
        }

        if (!wasBuffered) { lastBeatTimingPerc = Mathf.Abs(rhythmKeeper.validInputTimer); lastBeatTiming = rhythmKeeper.beatTiming; }

        eventCommunicator.DisableHitbox();
        blendLayers = false;
        comboTimer = -0.75f;
        isAttacking = true;
        canMove = false;
        AttackLayer();
        ourDeadTime = 0.333f;

        hitCanvasManager.SpawnHitCanvas(transform.position, lastBeatTiming);// message popup spawn

        if (isPoleDancer) { eventCommunicator.PickUpSpear(10); }
    }
    public void CanCancelAttack() { isAttacking = false; isLaunching = false; canMove = false; }
    public void EndAttack()
    {
        eventCommunicator.DisableHitbox();
        isAttacking = false;
        isLaunching = false;
        canMove = true;
        anim.ResetTrigger("Attack_1");
        anim.ResetTrigger("Attack_1_Released");
        anim.ResetTrigger("Attack_2");
        anim.ResetTrigger("Attack_2_Released");
        anim.ResetTrigger("Special");
        ResetLayers();
        if (isPoleDancer) { eventCommunicator.PickUpSpear(10); }
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
    public void BlendLayers() { blendLayers = true; }
    public void ResetLayers() 
    {
        //if (anim.GetCurrentAnimatorStateInfo(1).IsTag("Exit")) { BlendLayers(); return; }
        particle1.SetActive(false); particle2.SetActive(false);
        anim.SetLayerWeight(0, 1); 
        anim.SetLayerWeight(1, 0); 
        anim.SetLayerWeight(2, 0);
        anim.SetTrigger("DEFAULT"); 
    }
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
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void AddUltimateCharge(int amnt)
    {
        ultimateCharge += amnt;

        if (ultimateCharge >= ultimateLimit && !ultChargePlayed)
        {
            SoundPlayer.PlaySound(playerIndex, "Ult-Ready");
            ultChargePlayed = true;
        }
    }
    public void InLongCombo()
    {
        longCombo = true;
        comboTimer -= 0.4f;
        ourDeadTime += 0.4f;
    }
    public void GainSpecial(GameObject objectToKill)
    {
        specialCharges++;
        Destroy(objectToKill);
    }
    #endregion
    //Combat-related functions end

    //Start of update-related functions
    #region
    private void FixedUpdate()
    {
        if (blendLayers)
        {
            anim.SetLayerWeight(0, Mathf.Clamp(anim.GetLayerWeight(0) + Time.deltaTime * 2, 0, 1));
            anim.SetLayerWeight(1, Mathf.Clamp(anim.GetLayerWeight(1) - Time.deltaTime * 2, 0, 1));
            anim.SetLayerWeight(2, Mathf.Clamp(anim.GetLayerWeight(2) - Time.deltaTime * 2, 0, 1));
            if (blendLayers && anim.GetLayerWeight(0) == 1)
            {
                anim.SetTrigger("DEFAULT");
                EndAttack();
                blendLayers = false;
            }
        }

        if ((!isAttacking && canMove) || (canMove && isAttacking)) //If the player is not attacking or parrying, run general movement check
        {
            HandleMovement();
            HandleRotation();
            return;
        }

        
        if (isLaunching) //If the player needs to be launched, move them
        {
            charController.Move(launchDirection * Time.deltaTime);
        }
    }

    void Update()
    {
        ourDeadTime -= Time.deltaTime;
        comboTimer += Time.deltaTime;

        if (ourDeadTime < 0 && rhythmKeeper.beatTiming != "DeadZone" && triggerName != null) //If there is a buffered move, execute the move
        {
            longCombo = false;
            anim.SetTrigger(triggerName);
            triggerName = null;
            StartAttack(true);
        }


        if (hitStunRemaining > 0) //If in hitstun, skip rest of update
        {
            hitStunRemaining -= Time.deltaTime;
            Material[] mats = modelRenderer.materials;
            mats[matIndex] = hit;
            modelRenderer.materials = mats;
            if (takeKnockBack) { charController.Move(launchDirection * Time.deltaTime); } //Launch player over time if they are being knocked back
            return;
        }
        if (modelRenderer.materials[matIndex].name != normal.name + " (Instance)")
        {
            takeKnockBack = false;
            Material[] mats = modelRenderer.materials;
            mats[matIndex] = normal;
            modelRenderer.materials = mats;
            ResetLayers();
        }
    }

    void HandleMovement()
    {
        Vector3 move = Quaternion.Euler(0, sceneCamera.transform.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y);
        move.y = -2;
        charController.Move(move * Time.deltaTime * speed);

        if (move.x == 0 && move.z == 0) { anim.SetBool("Running", false); return; }

        anim.SetBool("Running", true);
    }

    void HandleRotation()
    {
        if (isGamepad && (movement.x != 0 || movement.y != 0))
        {
            Vector3 newMovement = Quaternion.Euler(0, sceneCamera.transform.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y);
            Quaternion newRotation = Quaternion.LookRotation(-newMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360);
            return;
        }
        if(!isGamepad)
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
    #endregion
    //End of update-related functions

    //Taking Damage & Status Effects
    #region
    public void TakeDamage(float damage, float hitStun, float knockBack, Vector3 hitBoxPos)
    {
        hitParticle.Play();
        speed = 2.5f;
        anim.SetTrigger("Hit");
        takeKnockBack = true;
        //SoundPlayer.PlaySound(playerIndex, "grunt");
        MiscLayer();
        isLaunching = false;
        isAttacking = false;
        hitStunRemaining = hitStun;
        HP -= damage * statusEffects.exposeStacks;
        Material[] mats = modelRenderer.materials;
        mats[matIndex] = hit;
        modelRenderer.materials = mats;
        Vector3 launchDir = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z) - hitBoxPos;
        launchDir.y = 0;
        launchDir.Normalize(); //knockback determines intensity of launch
        launchDirection = launchDir * knockBack;

        if (HP <= 0)
        {
            if (isPoleDancer) 
            { 
                SoundPlayer.PlaySound(playerIndex, $"death_Riven"); 
            }
            if (!isPoleDancer) 
            {
                SoundPlayer.PlaySound(playerIndex, $"death_Nova"); 
            }
            SoundPlayer.PlaySound(playerIndex, $"KNOCKOUT");
            win.playerIndex = playerIndex;
            win.WinScreen();
        }
    }
    #endregion

    //Misc & Coroutines
    #region
    private void SetOurDeadTime(string eventName, object param)
    {
        if (param.GetType() == typeof(float))
            ourDeadTime = Convert.ToSingle(param);
    }
    private void NewBeat(string eventName, object param)
    {
        if(anim.GetCurrentAnimatorStateInfo(1).IsName("Idle") || (isAttacking && comboTimer < 0 && longCombo))
            comboTimer = -0.5f;
    }

    public IEnumerator dodgeTiming()
    {
        if (isPoleDancer)
            eventCommunicator.PickUpSpear(10);
        canDodge = false;
        anim.SetTrigger("Dodge");
        yield return new WaitForSeconds(0.75f);
        ResetLayers();
        canDodge = true;
        canMove = true;
    }
    #endregion
}