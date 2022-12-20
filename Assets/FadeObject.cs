using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    public float fadeSpeed;

    void Update()
    {
        Color objectColor = GetComponent<Renderer>().material.color;

        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, objectColor.a - (fadeSpeed * Time.deltaTime));

        GetComponent<Renderer>().material.color = objectColor;

        if(objectColor.a <= 0) { Destroy(gameObject); }
    }
}
