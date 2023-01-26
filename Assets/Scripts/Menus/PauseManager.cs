using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public PauseMenu pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = FindObjectOfType<PauseMenu>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
