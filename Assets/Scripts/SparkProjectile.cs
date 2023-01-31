using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SparkProjectile : MonoBehaviour
{
    [SerializeField] float resistance;
    [SerializeField] float distance;
    [SerializeField] float travelTime;
    [SerializeField] float damageAsTrap;
    [SerializeField] Damage dmgScript;
    [SerializeField] GameObject orb;
    [SerializeField] GameObject shockwave;
    [SerializeField] VisualEffect sparks;
    [SerializeField] private int pulseFrequency = 8;

    private Vector3 direction;
    private bool isTrap;
    private bool wasPulsed;
    private RhythmBlinkingLights rhythmTracker;
    private int currentBeatsUntilPulse = 0;
    private float time;

    private void Start()
    {
        rhythmTracker = GameObject.FindObjectOfType<RhythmBlinkingLights>();
    }

    void Update()
    {
        if (rhythmTracker.spotlightGroupOne.activeInHierarchy && isTrap && orb.transform.localScale.x != 1 && dmgScript.dealtDamage == false && !wasPulsed)
        {
            wasPulsed = true;
            UpdatePulse();
            return;
        }
        if (isTrap && rhythmTracker.spotlightGroupTwo.activeInHierarchy && dmgScript.dealtDamage == false)
        {
            wasPulsed = false;
            dmgScript.gameObject.SetActive(false);
            SetOrbSize(0.5f);
            sparks.SetFloat("Electricity Size", 0);
            sparks.SetFloat("Electricity Size 2", 1f);
            return;
        }

        if (time >= travelTime & !isTrap)
        {
            time = 0;
            SoundPlayer.PlaySound(1, "riven_trap_stop");
            GetComponent<AudioSource>().Play();
            orb.SetActive(true);
            shockwave.SetActive(false);
            dmgScript.isTrap = true;
            dmgScript.baseDamage = damageAsTrap;
            dmgScript.dealtDamage = false;
            isTrap = true;
            return;
        }

        if (!isTrap)
        {
            time += Time.deltaTime;
            transform.Translate((Vector3.forward * Time.deltaTime * distance) - (new Vector3(0, Time.deltaTime / 4, 0)));
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3, 3), transform.position.y, Mathf.Clamp(transform.position.z, -2, 2));
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
        Debug.Log(currentBeatsUntilPulse);
        if (currentBeatsUntilPulse >= pulseFrequency)
        {
            SoundPlayer.PlaySound(1, "riven trap swell");
            sparks.SetFloat("Electricity Size", 4);
            sparks.SetFloat("Electricity Size 2", 5);
            currentBeatsUntilPulse = 0;
            dmgScript.gameObject.SetActive(true);
            SetOrbSize(1);
        }
    }
}
