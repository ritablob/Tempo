using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmBlinkingLights : MonoBehaviour
{
    public RhythmKeeper rhythmKeeper;
    public GameObject spotlightGroupOne;
    public GameObject spotlightGroupTwo;

    public UnityEvent onBeat;
    //public UnityEvent offBeat;

    private bool lightModeOne;
    private bool doOnce; // in case we want to do something once

    void Update()
    {
        if (rhythmKeeper.timingKey == "Perfect") // im not sure if checking when its "perfect" is ideal but i didnt wanna mess too much with rhythm keeper script
        {
            InvokeOnce(onBeat);
        }
        else
        {
            doOnce = false;
        }
    }
    void InvokeOnce(UnityEvent uevent) // in case we want to do something once pt. 2
    {
        if (!doOnce)
        {
            uevent.Invoke();
            doOnce = true;
        }
    }
    public void LightSwitch() // switches one group of lights off, the other switches on
    {
        if (!lightModeOne)
        {
            lightModeOne = true;
        }
        else
        {
            lightModeOne = false;
        }
        spotlightGroupOne.SetActive(lightModeOne);
        spotlightGroupTwo.SetActive(!lightModeOne);
    }
}
