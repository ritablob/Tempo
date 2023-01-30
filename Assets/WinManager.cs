using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public void WinScreen()
    {
        // all the stuff that needs to happen once win condition is met
    }
    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
