using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIngameMenu : MonoBehaviour
{
    public InputActionMap playerMap;
    private InGameManager inGameManager;
    PlayerMovement playerMovement;
    private void Start()
    {
        inGameManager = FindObjectOfType<InGameManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void ToggleInputActionMaps()
    {
        if (playerMap.enabled)
        {
            //localMap.Disable();
            playerMovement.playerInput.currentActionMap = playerMovement.playerInput.actions.actionMaps[1];
            Debug.Log("current action map = " + playerMovement.playerInput.currentActionMap);
            inGameManager.pauseMenu.SetActive(true);
        }
        else
        {
            //localMap.Enable();
            playerMovement.playerInput.currentActionMap = playerMovement.playerInput.actions.actionMaps[0];
            Debug.Log("current action map = " + playerMovement.playerInput.currentActionMap);
            inGameManager.pauseMenu.SetActive(false);
        }
        inGameManager.settingMenu.SetActive(false);
    }
    public void WinSetup()
    {
        if (playerMap.enabled)
        {
            playerMovement.playerInput.currentActionMap = playerMovement.playerInput.actions.actionMaps[1];
        }
    }
    public void SetPlayerMapBack()
    {
        playerMovement.playerInput.currentActionMap = playerMovement.playerInput.actions.actionMaps[0];
    }
    public void PauseMenuToggle(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            ToggleInputActionMaps();
        }
    }
}
