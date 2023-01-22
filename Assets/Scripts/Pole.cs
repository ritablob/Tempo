using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    float lerpAmnt;
    float lerpSpeed;
    Transform newPosition;

    public void Plant(GameObject polePosition)
    {
        //transform.parent = null;
        transform.parent = polePosition.transform;
        transform.position = polePosition.transform.position;
        transform.rotation = polePosition.transform.rotation;
    }
    public void LerpPole(Transform _newPosition, float _lerpSpeed)
    {
        lerpAmnt = 0;
        lerpSpeed = _lerpSpeed;
        newPosition = _newPosition;
    }

    private void Update()
    {
        lerpAmnt = Mathf.Clamp(lerpAmnt + (Time.deltaTime * lerpSpeed), 0, 1);

        if (lerpAmnt <= 1 && newPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition.position, lerpAmnt);
            transform.rotation = Quaternion.Lerp(transform.rotation, newPosition.rotation, lerpAmnt);
        }
    }
}
