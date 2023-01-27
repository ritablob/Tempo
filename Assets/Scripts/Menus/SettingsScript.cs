using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public List<Button> selectionButtonList;
    public AudioMixer audioMixer;
    public Slider slider;
    public Button exitButton;
    public Button firstButton;
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
    private void OnEnable()
    {
        slider.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        for (int i = 0; i < selectionButtonList.Count; i++)
        {
            if (selectionButtonList[i].gameObject.GetComponentInChildren<Pointer>() != null)
            {
                Pointer child = selectionButtonList[i].gameObject.GetComponentInChildren<Pointer>();
                Destroy(child.gameObject);
            }
            selectionButtonList[i].gameObject.SetActive(false);
        }
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(slider.gameObject);
    }
    private void OnDisable()
    {
        slider.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        for (int i = 0; i < selectionButtonList.Count; i++)
        {
            selectionButtonList[i].gameObject.SetActive(true);
        }
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(firstButton.gameObject);
    }
}
