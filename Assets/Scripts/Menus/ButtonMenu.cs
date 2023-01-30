using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    public List<Button> buttonList;
    public GameObject settingPrefab;
    public GameObject creditsObject;
    public Pointer pointer;
    public float delayTime = 0.5f; // small delay so that we can show that the button changes colour upon click
    private bool timerHasRunOut;
    private void Start()
    {
        settingPrefab.GetComponent<SettingsScript>().selectionButtonList = buttonList;
    }
    public void StartButton()
    {
        StartCoroutine(DelayStart(delayTime));
    }
    public void SettingButton()
    {
        StartCoroutine(DelaySettings(delayTime));
    }
    public void CreditButton()
    {
        StartCoroutine(DelayCredits(delayTime));
    }
    public void ExitButton()
    {
        StartCoroutine(DelayExit(delayTime));
    }
    IEnumerator DelayStart(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(1);
    }
    IEnumerator DelaySettings(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        settingPrefab.SetActive(true);
    }
    IEnumerator DelayCredits(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        creditsObject.SetActive(true);
    }
    IEnumerator DelayExit(float waitTime)
    {
        SoundPlayer.PlaySoundMenu("click");
        yield return new WaitForSeconds(waitTime);
        Application.Quit();
    }
}
