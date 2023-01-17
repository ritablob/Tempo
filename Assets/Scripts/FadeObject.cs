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

            float constant = fadeSpeed * Time.deltaTime;
            objectColor = new Color(objectColor.r - constant, objectColor.g + (constant / 2), objectColor.b + constant, objectColor.a - constant);

            if(objectColor.a <= 0) { Destroy(transform.parent.gameObject); }

            mat.color = objectColor;
        }
    }
}
