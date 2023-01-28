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
    private int ultimateLevel; // determines how many bars become visible
    private PlayerMovement playermovement;
    private void Start()
    {
        camera = FindObjectOfType<DynamicCamera>();
        doneSettingUp = false;
        slider2Children = slider2Parent.GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        if (isPlayerOneUI) // P1 UI
        {

            if (camera.player1 != null)
            {
                if (!doneSettingUp)
                {
                    SetUpPlayerUI();
                    playermovement = camera.player1.GetComponent<PlayerMovement>();
                    doneSettingUp = true;
                }
                UltimateSlider(playermovement);
            }
        }
        else // P2 UI
        {
            if (camera.player2 != null)
            {
                if (!doneSettingUp)
                {
                    SetUpPlayerUI();
                    playermovement = camera.player2.GetComponent<PlayerMovement>();
                    doneSettingUp = true;
                }
                UltimateSlider(playermovement);
            }
        }

    }

    void SetUpPlayerUI() // depending on which character player is using, UI is customized
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
        if (camera.player1.gameObject.name == "PoleDancer(Clone)")
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
    void UltimateSlider(PlayerMovement pMovement) // 
    {
        ultimateLevel = pMovement.ultimateCharge / 2;
        if (ultimateLevel < slider2Children.Length && ultimateLevel > 0)
        {
            for (int i = 0; i < ultimateLevel; i++) 
            {
                slider2Children[i].gameObject.SetActive(true);
            }
            for (int j = ultimateLevel; j < slider2Children.Length; j++)
            {
                slider2Children[j].gameObject.SetActive(false);
            }
        }
        else if (ultimateLevel > 0)
        {
            for (int i = 0; i < slider2Children.Length; i++)
            {
                slider2Children[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < slider2Children.Length; i++)
            {
                slider2Children[i].gameObject.SetActive(false);
            }
        }

    }
}
