using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    public Transform center; //Object to lerp to
    private Transform startPosition;

    public float lerpSpeed; //Based off of song beats per second

    public void Initialize(Transform _center, float _lerpSpeed)
    {
        center = _center;
        lerpSpeed = _lerpSpeed * 2;
        Debug.Log(_lerpSpeed);

        transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);

        startPosition = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(startPosition.position, center.position, lerpSpeed);
    }
}
