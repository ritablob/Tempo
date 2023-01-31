using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SparkProjectile : MonoBehaviour
{
    [SerializeField] float resistance;
    [SerializeField] float distance;
    [SerializeField] float speed;
    [SerializeField] float fallSpeed;
    [SerializeField] float damageAsTrap;
    [SerializeField] Damage dmgScript;
    [SerializeField] GameObject orb;
    [SerializeField] GameObject shockwave;
    [SerializeField] VisualEffect sparks;
    [SerializeField] private int pulseFrequency = 8;

    private Vector3 direction;
    private bool isTrap;
    private RhythmBlinkingLights rhythmTracker;
    private int currentBeatsUntilPulse = 0;
    private float time;

    private void Start()
    {
        rhythmTracker = GameObject.FindObjectOfType<RhythmBlinkingLights>();
    }

    void Update()
    {
        if (rhythmTracker.spotlightGroupOne.activeInHierarchy && isTrap && orb.transform.localScale.x != 1 && dmgScript.dealtDamage == false)
        {
            dmgScript.gameObject.SetActive(true);
       
            SetOrbSize(1);
            dmgScript.dealtDamage = false;
            UpdatePulse();
            return;
        }
        if (isTrap && rhythmTracker.spotlightGroupTwo.activeInHierarchy)
        {
            dmgScript.gameObject.SetActive(false);
            SetOrbSize(0.5f);
            UpdatePulse();
            return;
        }

        if (time >= 1)
        {
            time = 0;
            SoundPlayer.PlaySound(1, "riven_trap_stop");
            orb.SetActive(true);
            shockwave.SetActive(false);
            dmgScript.isTrap = true;
            dmgScript.baseDamage = damageAsTrap;
            dmgScript.dealtDamage = false;
            isTrap = true;
            speed = 0;
            return;
        }

        if (!isTrap)
        {
            time += Time.deltaTime;
            transform.Translate((Vector3.forward * Time.deltaTime * distance) - (new Vector3(0, Time.deltaTime / 4, 0)));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), transform.position.y, Mathf.Clamp(transform.position.z, -2, 2));

            speed -= resistance * Time.deltaTime;
            fallSpeed = Mathf.Clamp(fallSpeed - (resistance * (Time.deltaTime / 2)), 0, 999);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != dmgScript.playerRef.gameObject && other.tag == "Player" && isTrap)
        {
            return;
        }
        if (other.gameObject == dmgScript.playerRef.gameObject && other.tag == "Player" && isTrap)
        {
            GetComponent<SoundEffectPlayer>().PlaySound("riven trap pickup");
            dmgScript.playerRef.GainSpecial(this.gameObject);
        }
    }

    private void SetOrbSize(float size)
    {
        orb.transform.localScale = new Vector3(size, size, size);
    }

    public void SetEndPosition(PlayerMovement player)
    {
        dmgScript.playerRef = player;
    }

    private void UpdatePulse()
    {
        currentBeatsUntilPulse++;
        if (currentBeatsUntilPulse >= pulseFrequency)
        {
            SoundPlayer.PlaySound(1, "riven trap swell");
            sparks.SetFloat("Electricity Size", 4);
            sparks.SetFloat("Electricity Size 2", 5);
            currentBeatsUntilPulse = 0;
        }
        else
        {
            SoundPlayer.PlaySound(1, "riven_trap_activate");
            sparks.SetFloat("Electricity Size", 0);
            sparks.SetFloat("Electricity Size 2", 1f);
        }
    }
}
