using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Transform spawn1, spawn2;
    public GameObject char1, char2;

    public void _LoadLevel(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
    }

    public void SpawnCharacters()
    {
        var _object = Instantiate(char1, spawn1.position, spawn1.rotation);
        _object.transform.SetParent(spawn1);
        _object = Instantiate(char2, spawn2.position, spawn2.rotation);
        _object.transform.SetParent(spawn2);
    }
}
