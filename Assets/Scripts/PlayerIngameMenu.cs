using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIngameMenu : MonoBehaviour
{
    public InputActionMap localMap;
    private InGameManager inGameManager;
    private void Start()
    {
        inGameManager = FindObjectOfType<InGameManager>();
    }
    public void ToggleInputActionMaps()
    {
        if (localMap.enabled)
        {
            localMap.Disable();
            inGameManager.pauseMenu.SetActive(true);
        }
        else
        {
            localMap.Enable();
            inGameManager.pauseMenu.SetActive(false);
        }
        inGameManager.settingMenu.SetActive(false);
    }
    public void PauseMenuToggle(InputAction.CallbackContext ctx)
    {
        ToggleInputActionMaps();
    }
}
