using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShadowCloneInitializer : MonoBehaviour
{
    public void InitializeBones(Transform[] sourceBones)
    {
        Transform[] shadowBones = GetComponentsInChildren<Transform>();

        for (int i = 0; i < sourceBones.Length; i++)
        {
            shadowBones[i].localPosition = sourceBones[i].localPosition;
            shadowBones[i].localRotation = sourceBones[i].localRotation;
            shadowBones[i].localScale = sourceBones[i].localScale;
        }

        //gameObject.AddComponent<FadeObject>();
        //gameObject.GetComponent<FadeObject>().fadeSpeed = 1;
    }
}
