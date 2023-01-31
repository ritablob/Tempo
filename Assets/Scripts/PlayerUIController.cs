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
    public List<GameObject> projectiles;
    public Sprite knife;
    public GameObject portrait;
    [Header("0 - PoleDancer, 1 - BreakDancer")]
    public GameObject slider2Parent;
    public Image ultimateGlow;
    public float glowExpansionOnFullUltimate = 1.5f;
    public WinManager winRef;

    private DynamicCamera camera;
    private Transform[] slider2Children;
    private bool doneSettingUp;
    private float ultimateLevel; // determines how many bars become visible
    private PlayerMovement playerMovement;
    private float ultimateCharge;
    private float onePiece;
    float alpha;
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

        if (playerMovement != null)
        {
            UltimateSlider(playerMovement);
            foreach (GameObject thingy in projectiles)
            {
                thingy.SetActive(false);
            }
            for (int i = 0; i < playerMovement.specialCharges; i++)
            {
                projectiles[i].SetActive(true);
            }
        }


    }
    void SetUpPlayerUI(string objectName) // depending on which character player is using, UI is customized
    {

        if (objectName != "PoleDancer 2(Clone)" &&  objectName != "PoleDancer(Clone)")
        {
            foreach (GameObject proj in projectiles)
            {
                proj.GetComponent<Image>().sprite = knife;
            }
        }
    }
    public void ChangeIcon(Sprite icon)
    {
        portrait.GetComponent<Image>().sprite = icon;
    }
    public void SetSpecial()
    {

    }
    void UltimateSlider(PlayerMovement pMovement)
    {
        int barCount = (int)(pMovement.ultimateCharge / (pMovement.ultimateLimit * onePiece));
        if (barCount < 0)
        {
            barCount = 0;
        }
        alpha = onePiece * barCount;
        ultimateGlow.color = new Color(ultimateGlow.color.r, ultimateGlow.color.g, ultimateGlow.color.b, alpha);
        if (barCount >= 24)
        {
            ultimateGlow.transform.localScale = new Vector3(1f, glowExpansionOnFullUltimate, 1f);
        }
        else
        {
            ultimateGlow.transform.localScale = new Vector3(1f, 1f, 1f);
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
