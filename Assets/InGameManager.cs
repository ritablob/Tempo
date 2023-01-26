using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InGameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingMenu;

    public InputActionMap localMap;

    public void ToggleInputActionMaps()
    {
        if (localMap.enabled)
        {
            localMap.Disable();
            pauseMenu.SetActive(true);
        }
        else
        {
            localMap.Enable();
            pauseMenu.SetActive(false);
        }
        settingMenu.SetActive(false);
    }
    public void PauseMenuToggle(InputAction.CallbackContext ctx)
    {
        ToggleInputActionMaps();
    }
}
