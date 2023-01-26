using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{ 
    public GameObject settingPrefab;
    public Pointer pointer;
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
