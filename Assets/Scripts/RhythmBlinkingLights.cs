using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RhythmBlinkingLights : MonoBehaviour
{
    public RhythmKeeper rhythmKeeper;
    public GameObject spotlightParent;

    public UnityEvent onBeat;
    public UnityEvent offBeat;

    private bool lightsOff;
    private bool doOnce; // in case we want to do something once

    void Update()
    {
        if (rhythmKeeper.timingKey == "Perfect") // im not sure if checking when its "perfect" is ideal but i didnt wanna mess too much with rhythm keeper script
        {
            //InvokeOnce(onBeat);

            onBeat.Invoke();
        }
        else
        {
            //doOnce = false;

            offBeat.Invoke();
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
    public void LightsOn()
    {
        if (lightsOff)
        {
            lightsOff = false;
            spotlightParent.SetActive(true);
        }
    }
    public void LightsOff()
    {
        if (!lightsOff)
        {
            lightsOff = true;
            spotlightParent.SetActive(false);
        }
    }
    public void ToggleLight() // toggling light between states without on/off separation :)
    {
        lightsOff = !lightsOff;
        spotlightParent.SetActive(!spotlightParent.activeSelf);
    }
}
