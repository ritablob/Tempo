using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player_Menu : MonoBehaviour
{
    // is attached to character selection menu
    /*To-do:
     * - 4 characters (nova, nova alt., riven, riven alt.)
     * - move between scriptable objects -
     */
    private int charSelected;
    private float ignoreInputTime = 0.33f;
    private bool isReady;

    public PlayerConfigManager manager;
    public CharacterSelection[] characters;
    public Image nameImage;
    public Image portraitImage;
    public int playerID;


    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("PLAYERCONFIGMAN").GetComponent<PlayerConfigManager>();
        manager.SpawnSelectionMenu(this);
        ignoreInputTime = Time.time + ignoreInputTime;
        manager.SetPlayerCharacter(characters[charSelected].characterPrefab, playerID);
        nameImage.sprite = characters[charSelected].nameSprite;
        portraitImage.sprite = characters[charSelected].characterImageSprite;
        playerID = GetComponent<PlayerInput>().playerIndex;
    }

    private void OnLevelWasLoaded(int level)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(90909, 909);
        StartCoroutine(Delay());
    }

    public void Ready()
    {
        if(!isReady && manager.playerConfigs.Count == manager.maxPlayers)
        {
            manager.SetPlayerCharacter(characters[charSelected].characterPrefab, playerID);
            isReady = true; 
            manager.ReadyPlayer(playerID);
        }
    }
    public void UnReady()
    {
        if(isReady) 
        { 
            isReady = false; 
            manager.UnReadyPlayer(playerID);
        }
    }
    public void Left(InputAction.CallbackContext ctx) // switch between character profiles
    {
        if(charSelected > 0 && ctx.performed && !isReady) 
        {
            ignoreInputTime = Time.time + ignoreInputTime;
            charSelected--;
            manager.SetPlayerCharacter(characters[charSelected].characterPrefab, playerID);
            nameImage.sprite = characters[charSelected].nameSprite;
            portraitImage.sprite = characters[charSelected].characterImageSprite;
        }
        else if (ctx.performed && !isReady)
        {
            ignoreInputTime = Time.time + ignoreInputTime;
            charSelected = characters.Length - 1;
            manager.SetPlayerCharacter(characters[charSelected].characterPrefab, playerID);
            nameImage.sprite = characters[charSelected].nameSprite;
            portraitImage.sprite = characters[charSelected].characterImageSprite;
        }
    }
    public void Right(InputAction.CallbackContext ctx) // switch between character profiles
    {
        if (charSelected < characters.Length - 1 && ctx.performed && !isReady) 
        {
            ignoreInputTime = Time.time + ignoreInputTime;
            charSelected++;
            manager.SetPlayerCharacter(characters[charSelected].characterPrefab, playerID);
            nameImage.sprite = characters[charSelected].nameSprite;
            portraitImage.sprite = characters[charSelected].characterImageSprite;
        }
        else if (ctx.performed && !isReady)
        {
            ignoreInputTime = Time.time + ignoreInputTime;
            charSelected = 0;
            manager.SetPlayerCharacter(characters[charSelected].characterPrefab, playerID);
            nameImage.sprite = characters[charSelected].nameSprite;
            portraitImage.sprite = characters[charSelected].characterImageSprite;

        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
