using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Main script for playing sound effects listed in the SoundEffectsList. 
/// </summary>
public class SoundEffects : MonoBehaviour
{
    [Header("Please make sure both poledancer and breakdancer \nsound effects have the same names, i.e.\n 'dodge', 'hit_grunt', etc.")]
    [Space]
    public List<Sound> SoundEffectsPoledancer;
    public List<Sound> SoundEffectsBreakdancer;
    public float soundDestroyDelay = 0.3f;
    public AudioMixerGroup audioMixer;

    private static SoundEffects instance;
    public static SoundEffects Instance
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

/// <summary>
/// A way to set up sounds :)
/// </summary>
[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip audioClip;
    public bool loop = false;
    public float volume = 1f;
}


public static class SoundPlayer
{
    /// <summary>
    /// Plays any sound, as long as its defined within the Character sound lists. <br></br>
    /// Possible to manipulate volume and panning if one desires.
    /// Don't forget to add player index when playing a sound!
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="volume"></param>
    /// <param name="steroPan"></param>
    public static float setVolume;
    public static void PlaySound(int playerIndex, string soundName, float volume = 1, float steroPan = 0)
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray(); //Get an array of players from player config (0 = player 1, 1 = player 2)
        //Debug.Log(playerConfigs[0].objectName); //print the character name of player 1 gameobject. 
        if (SoundEffects.Instance)
        {

            Sound sound = GetSound(soundName, playerConfigs[playerIndex].character.name);
            //Debug.Log(playerConfigs[playerIndex].objectName);
            if (sound != null)
            {
                volume = setVolume;
                volume = 1;
                GameObject soundObject = new GameObject(soundName); // creates sound gameobject
                soundObject.transform.position = playerConfigs[playerIndex].character.transform.position;
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = SoundEffects.Instance.audioMixer;

                // sound settings in the SoundsList vvvv
                audioSource.volume = sound.volume;
                audioSource.loop = sound.loop;

                // special sound settings vvvv
                if (volume != 1)
                {
                    audioSource.volume = sound.volume * volume;
                }
                if (steroPan != 0)
                {
                    audioSource.panStereo = steroPan;
                }
                audioSource.clip = sound.audioClip;
                Debug.Log("Called, sound object " + soundObject);
                audioSource.Play(); // playing

                if (!audioSource.loop)
                {
                    Object.Destroy(soundObject, sound.audioClip.length + SoundEffects.Instance.soundDestroyDelay); // as long as its not looping, destroys once the sound is finished + delay
                }
            }
        }
    }
    /// <summary>
    /// Retrieves Sound from Soundslist based on its name. Make sure you know how to spell :)
    /// </summary>
    /// <param name="soundName"></param>
    /// <returns></returns>
    public static Sound GetSound(string soundName, string characterName)
    {
        if (characterName == "DEBUG CODE --- REMOVE LATER")
        {
            foreach (Sound sound in SoundEffects.Instance.SoundEffectsPoledancer)
            {
                if (sound.soundName == soundName)
                {
                    return sound;
                }
            }
        }
        else
        {
            foreach (Sound sound in SoundEffects.Instance.SoundEffectsBreakdancer)
            {
                if (sound.soundName == soundName)
                {
                    return sound;
                }
            }
        }    // XAVI: if its breakdancer, do with SoundEffectsBreakdancer

        Debug.LogError($"Sound '{soundName}' doesn't exist in the SoundsList, or your character name is set up wrong.");
        return null;
    }
    // ----------------- Menu player --------------------------
    public static void PlaySoundMenu(string soundName, float volume = 1, float steroPan = 0)
    {

        if (SoundEffectsMenu.Instance)
        {
            Debug.Log("call to playsoundMenu");
            Sound sound = GetSoundMenu(soundName);
            if (sound != null)
            {
                GameObject soundObject = new GameObject(soundName); // creates sound gameobject
                AudioSource audioSource = soundObject.AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = SoundEffectsMenu.Instance.audioMixer;

                // sound settings in the SoundsList vvvv
                audioSource.volume = sound.volume;
                audioSource.loop = sound.loop;

                // special sound settings vvvv
                if (volume != 1)
                {
                    audioSource.volume = sound.volume * volume;
                }
                if (steroPan != 0)
                {
                    audioSource.panStereo = steroPan;
                }
                audioSource.clip = sound.audioClip;
                audioSource.Play(); // playing

                if (!audioSource.loop)
                {
                    Object.Destroy(soundObject, sound.audioClip.length + SoundEffectsMenu.Instance.soundDestroyDelay); // as long as its not looping, destroys once the sound is finished + delay
                }
            }
            else
            {
                Debug.Log("null sound");
            }
        }
    }
    public static Sound GetSoundMenu(string soundName)
    {
        Debug.Log("found sound");
        foreach (Sound sound in SoundEffectsMenu.Instance.menuSounds)
        {
            if (sound.soundName == soundName)
            {


                return sound;
            }
        }  // XAVI: if its breakdancer, do with SoundEffectsBreakdancer

        Debug.LogError($"Sound '{soundName}' doesn't exist in the SoundsList, or your character name is set up wrong.");
        return null;
    }
    /// <summary>
    /// Just destroys the sound object you want.
    /// </summary>
    /// <param name="soundName"></param>
    public static void StopSound(string soundName)
    {
        GameObject GO = GameObject.Find(soundName);
        Object.Destroy(GO);
    }
}