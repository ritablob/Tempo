using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectsMenu : MonoBehaviour
{
    public List<Sound> menuSounds;
    private static SoundEffectsMenu instance;
    public float soundDestroyDelay = 0.3f;
    public AudioMixerGroup audioMixer;
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
