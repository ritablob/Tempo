using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public void _LoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
    }
}
