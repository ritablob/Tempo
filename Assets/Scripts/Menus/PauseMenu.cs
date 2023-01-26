using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingPrefab;
    public Pointer pointer;
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
