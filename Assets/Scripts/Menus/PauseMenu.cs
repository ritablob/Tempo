using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public List<Button> buttonList;
    public GameObject settingPrefab;
    public Pointer pointer;
    public Sprite originalBluePointer;
    public AudioMixer mixer;
    public InGameManager inGameManager;
    public PlayerIngameMenu[] playerIngameMenus;
    public float delayTime = 0.5f;


    private void Awake()
    {
        inGameManager.settingMenu = settingPrefab;
        inGameManager.pauseMenu = gameObject;
        settingPrefab.GetComponent<SettingsScript>().selectionButtonList = buttonList;

    }
    private void Start()
    {

        settingPrefab.SetActive(false);
        gameObject.SetActive(false);


    }
    private void OnEnable()
    {
        playerIngameMenus = FindObjectsOfType<PlayerIngameMenu>(); 
        settingPrefab.GetComponent<SettingsScript>().slider.gameObject.SetActive(false);
        settingPrefab.GetComponent<SettingsScript>().exitButton.gameObject.SetActive(false);
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
        StartCoroutine(DelayResume(delayTime));
    }
    public void RestartButton()
    {
        StartCoroutine(DelayRestart(delayTime));
    }
    public void SettingButton()
    {
        StartCoroutine(DelaySettings(delayTime));
    }
    public void QuitButton()
    {
        StartCoroutine(DelayQuit(delayTime));
    }
    IEnumerator DelayRestart(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator DelayResume(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < playerIngameMenus.Length; i++) // because resuming from the button screen (instead of clicking escape again)
                                                           // doesnt set the player maps back
        {
            playerIngameMenus[i].SetPlayerMapBack();
        }
        FindObjectOfType<Pointer>().GetComponent<Image>().sprite = originalBluePointer;
        gameObject.SetActive(false);

    }
    IEnumerator DelaySettings(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        settingPrefab.SetActive(true);
    }
    IEnumerator DelayQuit(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(0);
    }
}
