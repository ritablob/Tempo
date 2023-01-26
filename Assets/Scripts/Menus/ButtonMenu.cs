using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    public List<Button> buttonList;
    public GameObject settingPrefab;
    public Pointer pointer;

    private void Start()
    {
        settingPrefab.GetComponent<SettingsScript>().selectionButtonList = buttonList;
    }
    public void StartButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        StartCoroutine(WaitAndLoad(1f));
    }
    public void SettingButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        settingPrefab.SetActive(true);
    }
    public void CreditButton()
    {
        SoundPlayer.PlaySoundMenu("click");
    }
    public void ExitButton()
    {
        SoundPlayer.PlaySoundMenu("click");
        Application.Quit();
    }
    IEnumerator WaitAndLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(1);
    }
}
