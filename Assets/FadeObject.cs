using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObject : MonoBehaviour
{
    public float fadeSpeed;
    private Material[] mats;

    private void Awake()
    {
        mats = GetComponent<Renderer>().materials;
    }

    void Update()
    {
        foreach (Material mat in mats)
        {
            Color objectColor = mat.color;

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, objectColor.a - (fadeSpeed * Time.deltaTime));

            mat.color = objectColor;
        }
    }
}
