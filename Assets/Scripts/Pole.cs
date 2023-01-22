using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public void Plant(GameObject polePosition)
    {
        //transform.parent = null;
        transform.parent = polePosition.transform;
        transform.position = polePosition.transform.position;
        transform.rotation = polePosition.transform.rotation;
    }
    public void LerpPole(Transform newPosition)
    {
        transform.position = newPosition.position;
        transform.rotation = newPosition.rotation;
    }

}
