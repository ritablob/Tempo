using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public bool isPlayerOneUI;
    public TextMeshProUGUI text;
    public GameObject zap;
    public List<GameObject> knives;
    public GameObject portrait;
    [Header("0 - PoleDancer, 1 - BreakDancer")]
    public List<Sprite> portraitImages;
    public GameObject slider2Parent;

    private DynamicCamera camera;
    private Transform[] slider2Children;
    private bool doneSettingUp;
    private float ultimateLevel; // determines how many bars become visible
    private PlayerMovement playerMovement;
    private float ultimateCharge;
    private float onePiece;
    private void Start()
    {
        camera = new DynamicCamera();
        camera = FindObjectOfType<DynamicCamera>();
        doneSettingUp = false;
        slider2Children = slider2Parent.GetComponentsInChildren<Transform>();
        onePiece = 1.0f / slider2Children.Length;
        Debug.LogWarning(onePiece);
    }
    private void Update()
    {
        if (isPlayerOneUI && camera.player1 != null)
        {

            if (!doneSettingUp)
            {
                // CAMERA INSTEAD
                playerMovement = camera.player1.gameObject.GetComponent<PlayerMovement>();
                Debug.Log("player 1 " + playerMovement.gameObject.name);
                SetUpPlayerUI(playerMovement.gameObject.name);
                doneSettingUp = true;
            }
        }
        else if (camera.player2 != null)
        {
            if (!doneSettingUp)
            {
                playerMovement = camera.player2.gameObject.GetComponent<PlayerMovement>();
                Debug.Log("player 2 " + playerMovement.gameObject.name);
                SetUpPlayerUI(playerMovement.gameObject.name);
                doneSettingUp = true;
            }
        }

        UltimateSlider(playerMovement);
    }
    void SetUpPlayerUI(string objectName) // depending on which character player is using, UI is customized
    {
        int no;
        if (isPlayerOneUI)
        {
            no = 0;
        }
        else
        {
            no = 1;
        }
        if (objectName == "PoleDancer(Clone)")
        {
            text.text = "Riven";
            //portrait.GetComponent<Image>().sprite = portraitImages[0];
            zap.SetActive(true);
            knives[0].SetActive(false);
            knives[1].SetActive(false);
        }
        else
        {
            text.text = "Nova";
            //portrait.GetComponent<Image>().sprite = portraitImages[1];
            zap.SetActive(false);
            knives[0].SetActive(true);
            knives[1].SetActive(true);
        }
    }
    void UltimateSlider(PlayerMovement pMovement)
    {
        int barCount = (int)(pMovement.ultimateCharge / (pMovement.ultimateLimit * onePiece));
        if (barCount < 0)
        {
            barCount = 0;
        }
        //Debug.Log(pMovement.ultimateCharge+" "+pMovement.ultimateLimit + "Bar count for " + pMovement.gameObject.name + " " + barCount);
        for (int i = 0; i < slider2Children.Length; i++)
        {
            if (i <= barCount)
            {
                slider2Children[i].gameObject.SetActive(true);
            }
            else
            {
                slider2Children[i].gameObject.SetActive(false);
            }
        }
    }
}
