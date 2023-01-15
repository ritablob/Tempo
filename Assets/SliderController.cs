using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI UItext;
    public bool playerOne;
    public DynamicCamera dyncam;
    public Slider slider;
    void Update()
    {
        EditSlider();
    }
    void EditSlider()
    {
        if (playerOne && dyncam.player1 != null)
        {
            slider.value = dyncam.player1.GetComponent<PlayerMovement>().HP*0.01f;
            UItext.text = $"P1 - {dyncam.player1.GetComponent<PlayerMovement>().HP}hp";
        }
        else if (dyncam.player2 != null)
        {
            slider.value = dyncam.player2.GetComponent<PlayerMovement>().HP*0.01f;
            UItext.text = $"P2 - {dyncam.player1.GetComponent<PlayerMovement>().HP}hp";
        }
    }
}
