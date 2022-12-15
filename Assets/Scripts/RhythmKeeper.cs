using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RhythmKeeper : MonoBehaviour
{
    [Header("Music Related Vars")]
    [SerializeField] float beatsPerMinute;
    [SerializeField] float beatsPerBar;

    [Header("Display Related Vars")]
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Transform spawnLeft;
    [SerializeField] Transform spawnRight;
    [SerializeField] Transform leftArrow;
    [SerializeField] Transform rightArrow;
    [SerializeField] GameObject arrowToSpawn;
    [SerializeField] AudioSource audioSource;

    [Header("Gameplay Related Vars")]
    [SerializeField] float perfectLeewayPerc; //Leeway time for scoring a perfect
    [SerializeField] float normalLeewayPerc; //Leeway time for scoring a normal

    [HideInInspector]
    public string timingKey; //Used to test when actions occur in relation to the beat
    public string timingKeyCombo;

    private float beatLength;
    private float maxValidInputTime; //Keeps track of time
    private float validInputTimer; //Keeps track of beat percentage

    void Start()
    {
        beatLength = 60 / beatsPerMinute;
        maxValidInputTime = (beatLength * 4) / beatsPerBar;
        StartCoroutine(StartDelay((beatLength * 4) / beatsPerBar));
    }

    private void Update()
    {
        validInputTimer += Time.deltaTime;
        float beatPerc = validInputTimer / maxValidInputTime * 100;

        if(beatPerc < normalLeewayPerc) { timingKey = "Miss"; }
        else if(beatPerc >= normalLeewayPerc && beatPerc < perfectLeewayPerc) { timingKey = "Early"; }
        else if(beatPerc >= perfectLeewayPerc && beatPerc < 100) { timingKey = "Perfect"; }
        else { timingKey = "Miss"; }
    }

    private void FixedUpdate()
    {
        txt.text = timingKey;
    }

    IEnumerator StartDelay(float waitTime)
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(WaitForBeat(waitTime));
        validInputTimer = 0;
    }

    IEnumerator WaitForBeat(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); //After waiting, spawn and set up 2 arrow objects

        validInputTimer = 0;

        GameObject arrow = Instantiate(arrowToSpawn, spawnLeft.position, spawnLeft.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(leftArrow, beatLength);

        arrow = Instantiate(arrowToSpawn, spawnRight.position, spawnRight.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(rightArrow, beatLength);

        StartCoroutine(WaitForBeat((beatLength * 4) / beatsPerBar));
    }
}
