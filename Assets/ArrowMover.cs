using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    public Transform center; //Object to lerp to
    private Transform startPosition;

    public float lerpSpeed; //Based off of song beats per second
    private float startTime;
    private float lerpDistance;

    public void Initialize(Transform _center, float _lerpSpeed)
    {
        center = _center;
        lerpSpeed = _lerpSpeed * 2;
        Debug.Log(_lerpSpeed);

        transform.parent = GameObject.FindObjectOfType<Canvas>().transform;

        startPosition = gameObject.transform;
        lerpDistance = Vector3.Distance(startPosition.position, center.position);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeProgress = (Time.time - startTime) / lerpSpeed;

        float lerpProgress = timeProgress / lerpDistance;

        transform.position = Vector3.Lerp(startPosition.position, center.position, lerpProgress);
    }
}
