using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFocus : MonoBehaviour
{
    public PlayerMovement playerRef;
    Light lightComponent;

    private void Start()
    {
        lightComponent = GetComponent<Light>();
    }

    private void Update()
    {
        float beatPerc = playerRef.validInputTimer / playerRef.maxValidInputTime * 100;

        lightComponent.spotAngle = 55 - (beatPerc / 2);
    }
}
