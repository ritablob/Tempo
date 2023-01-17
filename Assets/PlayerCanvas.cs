using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    public StatusEffects saffects;
    public GameObject stunImage;
    public GameObject bloodImage;
    Camera sceneCamera;

    // Start is called before the first frame update
    void Start()
    {
        sceneCamera = Camera.main;
        stunImage.SetActive(false);
        bloodImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - sceneCamera.transform.position;
        transform.LookAt(direction);
        StunnedImage();
    }

    void StunnedImage()
    {
        if (saffects.stunStacks > 0)
        {
            stunImage.SetActive(true);
        }
        else
        {
            stunImage.SetActive(false);
        }
    }
}
