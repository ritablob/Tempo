using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKeeper : MonoBehaviour
{
    [Header("Music Related Vars")]
    [SerializeField] AudioSource[] AudioSources;
    [SerializeField] float beatsPerMinute;
    [SerializeField] float beatsPerBar;

    //[HideInInspector]
    public string beatTiming;
    public float beatLengthThird;
    public float maxValidInputTime; //Keeps track of max input time
    public float validInputTimer; //Keeps track of beat percentage

    [HideInInspector] public bool isEarly;

    void Start()
    {
        //Calculate beat rate
        float beatLength = Mathf.Abs(60 / beatsPerMinute);
        beatLengthThird = Mathf.Abs(beatLength / 3);
        maxValidInputTime = Mathf.Abs((beatLength * 4) / beatsPerBar);
        StartCoroutine(StartDelay(Mathf.Abs((beatLength * 4 / beatsPerBar * 4) - (beatLengthThird * 1.5f)))); //Skew the rhythm tracker to start counting at 'early' timing
    }

    private void Update() //Calculate the current timing key
    {
        validInputTimer += Mathf.Abs(Time.deltaTime); //Count elapsed time in beat
        float difference = Mathf.Abs(validInputTimer);

        if (difference < 0.0855f)
        {
            isEarly = false;
            beatTiming = "Perfect";
        }
        else if (difference > 0.09f && difference < 0.1777f)
        {
            if (validInputTimer < 0)
            {
                isEarly = true;
            }
            else
            {
                isEarly = false;
            }
            beatTiming = "Near";
        }
        else
        {
            beatTiming = "DeadZone";
        }

    }

    IEnumerator StartDelay(float waitTime) //Delay music & beats for a bit
    {
        yield return new WaitForSeconds(waitTime);
        foreach(AudioSource audioSource in AudioSources)
        {
            audioSource.Play();
        }
        StartCoroutine(WaitForBeat(waitTime));
        EventSystem.Instance.Fire("DeadTime", "Apply", beatLengthThird);
        validInputTimer = beatLengthThird * -1.5f; //Set the input timer to count early
    }

    IEnumerator WaitForBeat(float waitTime) //Wait for a bit before spawning the next 2 arrows
    {
        yield return new WaitForSeconds(waitTime);
        EventSystem.Instance.Fire("New Beat", "", null);
        validInputTimer = beatLengthThird * -1.5f; //Set the input timer to count early

        float beatLength = Mathf.Abs(60 / beatsPerMinute);
        StartCoroutine(WaitForBeat(Mathf.Abs((beatLength * 4) / beatsPerBar) - 0.005f));
    }
}
