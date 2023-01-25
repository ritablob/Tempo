using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    public List<Button> buttonOrderList;
    public GameObject pointer;
    public GameObject settingPrefab;

    private int buttonIndex = 0;
    void ToggleButtons()
    {
        //if (Input.up) // how to get device down input?
        {
            // move up the list with up
            if (buttonIndex > 0)
                buttonIndex--;
            else
                buttonIndex = buttonOrderList.Count - 1;
            
        }
        //else if (Input.down) 
        { 
            // move down the list with down
            if (buttonIndex < buttonOrderList.Count)
                buttonIndex++;
            else
                buttonIndex = 0;

        }
        // spawn arrow and assign as child to button
        Instantiate(pointer, buttonOrderList[buttonIndex].gameObject.transform);
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
    public void AboutButton()
    {
        SoundPlayer.PlaySoundMenu("click");
    }
    public void CreditsButton()
    {
        Debug.Log("pressed credits");
        SoundPlayer.PlaySoundMenu("click");
    }
    IEnumerator WaitAndLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(1);
    }
}
