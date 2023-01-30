using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public PlayerMovement playerRef;
    private DynamicCamera camRef;
    public bool dealtDamage;

    [Header("Hit Values")]
    [SerializeField] float baseDamage;
    [SerializeField] float hitStun;
    [SerializeField] float knockBack;

    [Header("Hit Effects")]
    [SerializeField] bool isTrap;
    [SerializeField] bool exposing;
    [SerializeField] bool stunning;
    [SerializeField] bool bleeding;
    [SerializeField] bool weaken;

    private void Start()
    {
        camRef = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DynamicCamera>();
    }
    private void OnEnable()
    {
        dealtDamage = false;
    }
    public void Initialize(PlayerMovement player)
    {
        playerRef = player;
    }
    private void OnDisable()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTrap && other.tag == "Player" && other.gameObject.GetComponent<PlayerMovement>() != playerRef && !dealtDamage)
        {
            dealtDamage = true;

            Vector3 launchPoint = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

            //Apply damage
            other.GetComponent<PlayerMovement>().TakeDamage(baseDamage, hitStun, knockBack, launchPoint);
            other.GetComponent<PlayerMovement>().AddUltimateCharge(Mathf.RoundToInt(baseDamage / 5)); //Give enemy come-back ult charge

            PlayHitSound(other, baseDamage);
            return;
        }

        if(other.tag == "Player" && other.gameObject.GetComponent<PlayerMovement>() != playerRef && !dealtDamage)
        {
            dealtDamage = true;

            Vector3 launchPoint = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

            //Apply damage modifier based on timing
            float modifier = Mathf.Clamp(1 - (playerRef.lastBeatTimingPerc * 10), 0.2f, 1);
            float ultChargeModifier = Mathf.Clamp(1 - (playerRef.lastBeatTimingPerc * 10), 0.7f, 1);
            modifier += other.GetComponent<StatusEffects>().exposeStacks / 2;
            float newDamage = baseDamage * modifier;
            newDamage -= playerRef.gameObject.GetComponent<StatusEffects>().weaknessStacks;

            //Apply attack effects
            if (exposing) { other.GetComponent<StatusEffects>().AddExpose(); }
            if (stunning) { other.GetComponent<StatusEffects>().AddStun(); }
            if (bleeding) { other.GetComponent<StatusEffects>().AddBleed(); }
            if (weaken) { other.GetComponent<StatusEffects>().AddWeakness(); }

            //Apply damage
            other.GetComponent<PlayerMovement>().TakeDamage(newDamage, hitStun, knockBack, launchPoint);
            playerRef.AddUltimateCharge(Mathf.RoundToInt(baseDamage * ultChargeModifier)); //Give attacker ult charge
            other.GetComponent<PlayerMovement>().AddUltimateCharge(Mathf.RoundToInt((baseDamage * ultChargeModifier) / 5)); //Give enemy come-back ult charge

            PlayHitSound(other, newDamage);
        }
    }

    private void PlayHitSound(Collider other, float newDamage)
    {        
        //Play hit sound based on attacker
        if (playerRef.isPoleDancer) { SoundPlayer.PlaySound(playerRef.playerIndex, "Riven_Attack_Hit"); }
        if (!playerRef.isPoleDancer && !playerRef.longCombo) { SoundPlayer.PlaySound(playerRef.playerIndex, "Nova_Attack_Hit"); }
        if(!playerRef.isPoleDancer && playerRef.longCombo) { SoundPlayer.PlaySound(playerRef.playerIndex, "Nova_Attack_Kick"); }

        //Play hit sound based on opponent
        if (newDamage < 6 && other.GetComponent<PlayerMovement>().isPoleDancer) { SoundPlayer.PlaySound(playerRef.playerIndex, "Riven_Light"); return; }
        if (newDamage >= 6 && other.GetComponent<PlayerMovement>().isPoleDancer) { SoundPlayer.PlaySound(playerRef.playerIndex, "Riven_Heavy"); return; }
        if (newDamage < 6 && !other.GetComponent<PlayerMovement>().isPoleDancer) { SoundPlayer.PlaySound(playerRef.playerIndex, "Nova_Light"); return; }
        if (newDamage >= 6 && !other.GetComponent<PlayerMovement>().isPoleDancer) { SoundPlayer.PlaySound(playerRef.playerIndex, "Nova_Heavy"); }

    }
}
