using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKeeper : MonoBehaviour
{
    [Header("Music Related Vars")]
    [SerializeField] float beatsPerMinute;
    [SerializeField] float beatsPerBar;

    [Header("Display Related Vars")]
    [SerializeField] Transform spawnLeft;
    [SerializeField] Transform spawnRight;
    [SerializeField] Transform leftArrow;
    [SerializeField] Transform rightArrow;
    [SerializeField] GameObject arrowToSpawn;
    [SerializeField] AudioSource audioSource;

    [Header("Gameplay Related Vars")]
    [SerializeField] float perfectLeeway; //Leeway time for scoring a perfect
    [SerializeField] float normalLeeway; //Leeway time for scoring a normal

    //[HideInInspector]
    public string timingKey; //Used to test when actions occur in relation to the beat

    private float beatLength;
    private float validInputTimer; //Keeps track of time

    void Start()
    {
        beatLength = 60 / beatsPerMinute;
        validInputTimer = ((beatLength * 4) / beatsPerBar) *-1;
        validInputTimer += normalLeeway;
        StartCoroutine(WaitForBeat((beatLength * 4) / beatsPerBar));
    }

    private void Update()
    {
        validInputTimer += Time.deltaTime;

        if(validInputTimer < normalLeeway * -1) { timingKey = "Miss"; } //If beat is before the normal timing, count as miss
        else if(validInputTimer >= normalLeeway * -1 && validInputTimer < perfectLeeway * -1) { timingKey = "Early"; } //If beat is on normal timing, count as hit
        else if(validInputTimer >= perfectLeeway * -1 && validInputTimer <= perfectLeeway) { timingKey = "Perfect"; } //If beat is between perfect leewats, count as perfect
        else if (validInputTimer > perfectLeeway && validInputTimer <= normalLeeway) { timingKey = "Late"; } //If beat is between perfect leewats, count as perfect
        else //If the player misses the beat, reset timer
        {
            validInputTimer = ((beatLength * 4) / beatsPerBar) * -1;
            validInputTimer += normalLeeway;
            timingKey = "Miss";
        }
    }

    IEnumerator WaitForBeat(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); //After waiting, spawn and set up 2 arrow objects
        GameObject arrow = Instantiate(arrowToSpawn, spawnLeft.position, spawnLeft.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(leftArrow, beatLength);

        arrow = Instantiate(arrowToSpawn, spawnRight.position, spawnRight.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(rightArrow, beatLength);
        StartCoroutine(WaitForBeat((beatLength * 4) / beatsPerBar));
    }
}
