using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("Player 1")]
    public TextMeshProUGUI text1;
    public GameObject zap1;
    public List<GameObject> knives1;
    public GameObject portrait1;
    public GameObject ultimateSliderParent1;

    [Header("Player 2")]
    public TextMeshProUGUI text2;
    public GameObject zap2;
    public List<GameObject> knives2;
    public GameObject portrait2;
    public GameObject ultimateSliderParent2;


    //private DynamicCamera camera;
    private Transform[] ultimateChildren1;
    private Transform[] ultimateChildren2;
    private bool doneSettingUp;
    private float ultimateLevel1;
    private float ultimateLevel2;// determines how many bars become visible
    //private PlayerMovement playermovement;
    private void Start()
    {
        //camera = FindObjectOfType<DynamicCamera>();
        doneSettingUp = false;
        //slider2Children = slider2Parent.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        //var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        //if (playerConfigs != null)
        //{
        //   // if (isPlayerOneUI)
        //    {
        //        if (!doneSettingUp)
        //        {
        //            playerConfigs[0].playerObject.GetComponent<PlayerMovement>();
        //            Debug.Log("player 1 " + playerConfigs[0].objectName);
        //            SetUpPlayerUI(playerConfigs[0].objectName);
        //            doneSettingUp = true;
        //        }
        //    }
        //    else
        //    {
        //        if (!doneSettingUp)
        //        {
        //            playerConfigs[1].playerObject.GetComponent<PlayerMovement>();
        //            Debug.Log("player 2 " + playerConfigs[1].objectName);
        //            SetUpPlayerUI(playerConfigs[1].objectName);
        //            doneSettingUp = true;
        //        }
        //    }
        //}
        //if (this.isPlayerOneUI) // P1 UI
        //{

        //    if (camera.player1 != null)
        //    {
        //        if (!doneSettingUp)
        //        {
        //            SetUpPlayerUI();
        //            playermovement = camera.player1.GetComponent<PlayerMovement>();
        //            doneSettingUp = true;
        //        }
        //        UltimateSlider(playermovement);
        //    }
        //}
        //else // P2 UI
        //{
        //    if (camera.player2 != null)
        //    {
        //        if (!doneSettingUp)
        //        {
        //            SetUpPlayerUI();
        //            playermovement = camera.player2.GetComponent<PlayerMovement>();
        //            doneSettingUp = true;
        //        }
        //        UltimateSlider(playermovement);
        //    }
        //}

    }

    void SetUpPlayerUI(string objectName) // depending on which character player is using, UI is customized
        // BUG: both players are based off of player 1
    {
//        int no;
////if (isPlayerOneUI)
//        {
//            no = 0;
//        }
//        else
//        {
//            no = 1;
//        }
        if (objectName == "PoleDancer(Clone)")
        {
          //  text.text = "Riven";
            //portrait.GetComponent<Image>().sprite = portraitImages[0];
          //  zap.SetActive(true);
         //   knives[0].SetActive(false);
         //   knives[1].SetActive(false);
        }
        else
        {
          //  text.text = "Nova";
            //portrait.GetComponent<Image>().sprite = portraitImages[1];
          //  zap.SetActive(false);
          //  knives[0].SetActive(true);
         //   knives[1].SetActive(true);
        }
    }
    void UltimateSlider(PlayerMovement pMovement) // 
    {

        //ultimateLevel = pMovement.ultimateLimit > 0 ? pMovement.ultimateCharge / pMovement.ultimateLimit : 1f; // fullness percentage (0.0 - 1.0)

        //int barCount = Convert.ToInt32(slider2Children.Length * Math.Round(ultimateLevel, 1));
        //if (ultimateLevel < 1 && ultimateLevel > 0)
        //{
        //    for (int i = 0; i <= barCount; i++) 
        //    {
        //        slider2Children[i].gameObject.SetActive(true);
        //    }
        //    for (int j = barCount; j < slider2Children.Length; j++)
        //    {
        //        slider2Children[j].gameObject.SetActive(false);
        //    }
        //}
        //else if (ultimateLevel > 0)
        //{
        //    for (int i = 0; i < slider2Children.Length; i++)
        //    {
        //        slider2Children[i].gameObject.SetActive(true);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < slider2Children.Length; i++)
        //    {
        //        slider2Children[i].gameObject.SetActive(false);
        //    }
        //}

    }
}
