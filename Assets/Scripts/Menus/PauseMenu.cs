using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingPrefab;
    public Pointer pointer;
    public AudioMixer mixer;
    private void Start()
    {
        settingPrefab.SetActive(false);
        gameObject.SetActive(false);

    }
    private void OnEnable()
    {
        mixer.SetFloat("EQgain", 0f);
        /* - lights and music continue to flicker 
         * - music muffled
         * - player input disabled 
         */
        //ime.timeScale = 0f;
    }
    private void OnDisable()
    {
        mixer.SetFloat("EQgain", 1f);
        //Time.timeScale = 1f;
    }
    public void ResumeButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        gameObject.SetActive(false);
    }
    public void RestartButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SettingButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        settingPrefab.SetActive(true);
    }
    public void QuitButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        StartCoroutine(WaitAndLoadMenu(1));

    }
    IEnumerator WaitAndLoadMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(0);
    }
}
