using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RhythmKeeper : MonoBehaviour
{
    [Header("Music Related Vars")]
    [SerializeField] AudioSource[] AudioSources;
    [SerializeField] float beatsPerMinute;
    [SerializeField] float beatsPerBar;

    [Header("Display Related Vars")]
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Transform spawnLeft;
    [SerializeField] Transform spawnRight;
    [SerializeField] Transform leftArrow;
    [SerializeField] Transform rightArrow;
    [SerializeField] GameObject arrowToSpawn;

    [Header("Gameplay Related Vars")]
    public float perfectLeewayPerc; //Leeway time for scoring a perfect
    public float normalLeewayPerc; //Leeway time for scoring a normal

    [HideInInspector]
    public string timingKey; //Used to test when actions occur in relation to the beat
    public float beatLength;

    private float maxValidInputTime; //Keeps track of time
    private float validInputTimer; //Keeps track of beat percentage

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

        //Apply timing key (for easy access)
        if(beatPerc < normalLeewayPerc) { timingKey = "Miss"; }
        else if(beatPerc >= normalLeewayPerc && beatPerc < perfectLeewayPerc) { timingKey = "Early"; }
        else if(beatPerc >= perfectLeewayPerc && beatPerc < 100) { timingKey = "Perfect"; }
        else { timingKey = "Miss"; }
    }

    private void FixedUpdate()
    {
        txt.text = timingKey;
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
        Debug.Log("jsbnjdfb");
        validInputTimer = 0;

        GameObject arrow = Instantiate(arrowToSpawn, spawnLeft.position, spawnLeft.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(leftArrow, beatLength * 2, true);

        arrow = Instantiate(arrowToSpawn, spawnRight.position, spawnRight.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(rightArrow, beatLength * 2, true);

        StartCoroutine(WaitForBeat((beatLength * 4) / beatsPerBar));
    }

    public void SpawnArrow(float lerpSpeed, int playerIndex) //Spawns an arrow. Called from player movement
    {
        Color newTint = new Color(0, 0, 255);
        if(playerIndex == 1) { newTint = new Color(255, 0, 0); }

        //Spawn two arrows
        GameObject arrow = Instantiate(arrowToSpawn, spawnLeft.position, spawnLeft.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(leftArrow, lerpSpeed, false);
        arrow.GetComponent<Image>().color = newTint;

        arrow = Instantiate(arrowToSpawn, spawnRight.position, spawnRight.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(rightArrow, lerpSpeed, false);
        arrow.GetComponent<Image>().color = newTint;
    }
}
