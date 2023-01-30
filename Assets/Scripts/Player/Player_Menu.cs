using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player_Menu : MonoBehaviour
{
    private int charSelected;
    private float ignoreInputTime = 0.33f;
    private bool isReady;

    public PlayerConfigManager manager;
    public GameObject playerIcon;
    public Image preview;
    public TextMeshProUGUI charName;
    public int playerID;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerConfigManager>();
        manager.SpawnPlayerIcon(this);
        ignoreInputTime = Time.time + ignoreInputTime;
        manager.SetPlayerCharacter(charSelected, playerID);
        preview.sprite = manager.charSelectPositions[charSelected].GetComponent<Image>().sprite;
        preview.color = new Color(255, 255, 255, 255);
        charName.text = manager.charSelectPositions[charSelected].name;
    }

    public void Ready()
    {
        if(!isReady) 
        { 
            isReady = true; 
            preview.gameObject.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            manager.ReadyPlayer(playerID);
        }
    }
    public void UnReady()
    {
        if(isReady) 
        { 
            isReady = false; 
            preview.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            manager.UnReadyPlayer(playerID);
        }
    }
    public void Left(InputAction.CallbackContext ctx) //Switch between two portraits
    {
        if(charSelected > 0 && ctx.performed && !isReady) 
        {
            ignoreInputTime = Time.time + ignoreInputTime;
            charSelected--;
            manager.SetPlayerCharacter(charSelected, playerID);
            playerIcon.transform.SetParent(manager.charSelectPositions[charSelected].transform);
            preview.sprite = manager.charSelectPositions[charSelected].GetComponent<Image>().sprite;
            charName.text = manager.charSelectPositions[charSelected].name;
        }
    }
    public void Right(InputAction.CallbackContext ctx) //
    {
        if (charSelected < manager.charSelectPositions.Length - 1 && ctx.performed && !isReady) 
        {
            ignoreInputTime = Time.time + ignoreInputTime;
            charSelected++;
            manager.SetPlayerCharacter(charSelected, playerID);
            playerIcon.transform.SetParent(manager.charSelectPositions[charSelected].transform);
            preview.sprite = manager.charSelectPositions[charSelected].GetComponent<Image>().sprite;
            charName.text = manager.charSelectPositions[charSelected].name;
        }
    }
}
