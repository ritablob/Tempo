using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI UItext;
    public bool playerOne;
    public DynamicCamera dyncam;
    public Slider slider;
    public float sliderMultiplier;
    bool hasSetUp = false;
    void Update()
    {
        SetUp();
        EditSlider();
    }
    void EditSlider()
    {
        if (playerOne && dyncam.player1 != null)
        {
            slider.value = dyncam.player1.GetComponent<PlayerMovement>().HP*sliderMultiplier;
            //UItext.text = $"P1 - {dyncam.player1.GetComponent<PlayerMovement>().HP}hp";
        }
        else if (dyncam.player2 != null)
        {
            slider.value = dyncam.player2.GetComponent<PlayerMovement>().HP*sliderMultiplier;
            //UItext.text = $"P2 - {dyncam.player1.GetComponent<PlayerMovement>().HP}hp";
        }
    }
    void SetUp()
    {
        if (!hasSetUp)
        {
            if (playerOne && dyncam.player1 != null)
            {
                sliderMultiplier = 1f / dyncam.player1.GetComponent<PlayerMovement>().HP;
                hasSetUp = true;
            }
            else if (dyncam.player2 != null)
            {
                sliderMultiplier = 1f / dyncam.player2.GetComponent<PlayerMovement>().HP;
                hasSetUp = true;
            }
        }
    }
}
