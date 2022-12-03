using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKeeper : MonoBehaviour
{
    [Header("Music Related Vars")]
    [SerializeField] float beathLength;
    [SerializeField] float beatsPerBar;

    [Header("Display Related Vars")]
    [SerializeField] Transform spawnLeft;
    [SerializeField] Transform spawnRight;
    [SerializeField] Transform center;
    [SerializeField] GameObject arrowToSpawn;

    void Start()
    {
        StartCoroutine(WaitForBeat((beathLength * 4) / beatsPerBar));
    }

    IEnumerator WaitForBeat(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); //After waiting, spawn and set up 2 arrow objects
        GameObject arrow = Instantiate(arrowToSpawn, spawnLeft.position, spawnLeft.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(center, beathLength);
        arrow = Instantiate(arrowToSpawn, spawnRight.position, spawnRight.rotation);
        arrow.GetComponent<ArrowMover>().Initialize(center, beathLength);
        StartCoroutine(WaitForBeat((beathLength * 4) / beatsPerBar));
    }
}
