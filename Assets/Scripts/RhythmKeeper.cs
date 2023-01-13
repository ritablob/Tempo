using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKeeper : MonoBehaviour
{
    [Header("Music Related Vars")]
    [SerializeField] AudioSource[] AudioSources;
    [SerializeField] float beatsPerMinute;
    [SerializeField] float beatsPerBar;

    [HideInInspector]
    public float beatLength;
    public float maxValidInputTime; //Keeps track of time
    public float validInputTimer; //Keeps track of beat percentage

    void Start()
    {
        //Calculate beat rate
        beatLength = 60 / beatsPerMinute;
        maxValidInputTime = (beatLength * 4) / beatsPerBar;
        StartCoroutine(StartDelay((beatLength * 4 / beatsPerBar)));
    }

    private void Update() //Calculate the current timing key
    {
        validInputTimer += Time.deltaTime; //Count elapsed time in beat
        float beatPerc = validInputTimer / maxValidInputTime * 100; //Calculate percentage of beat
    }

    IEnumerator StartDelay(float waitTime) //Delay music & beats for a bit
    {
        yield return new WaitForSeconds(waitTime * 4);
        foreach(AudioSource audioSource in AudioSources)
        {
            audioSource.Play();
        }
        StartCoroutine(WaitForBeat(waitTime));
        validInputTimer = 0;
    }

    IEnumerator WaitForBeat(float waitTime) //Wait for a bit before spawning the next 2 arrows
    {
        yield return new WaitForSeconds(waitTime); //After waiting, spawn and set up 2 arrow objects
        validInputTimer = 0;

        StartCoroutine(WaitForBeat((beatLength * 4) / beatsPerBar));
    }
}
