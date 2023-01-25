using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;
    private void Start()
    {
        slider.value = 1f;
        audioMixer.SetFloat("volume", 0);
    }
    public void VolumeChange(float sliderValue)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(sliderValue) * 20);
    }
    public void ExitButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        gameObject.SetActive(false);
    }
}
