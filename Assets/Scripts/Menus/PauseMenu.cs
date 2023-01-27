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
    public AudioMixer mixer;
    public InGameManager inGameManager;
    public GameObject inGameUI;
    public PlayerIngameMenu[] playerIngameMenus;

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
        inGameUI.SetActive(false);
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
        inGameUI.SetActive(true);
        mixer.SetFloat("EQgain", 1f);
        //Time.timeScale = 1f;
    }
    public void ResumeButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        for (int i = 0; i < playerIngameMenus.Length; i++) // because resuming from the button screen (instead of clicking escape again)
                                                           // doesnt set the player maps back
        {
            playerIngameMenus[i].SetPlayerMapBack();
        }
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
