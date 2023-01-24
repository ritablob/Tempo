using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsMenu : MonoBehaviour
{
    public List<Sound> menuSounds;
    private static SoundEffectsMenu instance;
    public float soundDestroyDelay = 0.3f;
    public static SoundEffectsMenu Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
