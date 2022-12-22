using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectsManager : MonoBehaviour
{
    [SerializeField] AudioSource music;
    
    bool toggle;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            toggle = !toggle;
            if(toggle) { PauseGame(); }
            else { UnPauseGame(); }
        }

        if (!music.isPlaying) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }

    public void PauseGame()
    {
        music.Pause();
    }

    public void UnPauseGame()
    {
        music.UnPause();
    }
}
