using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private GameObject playerToSpawn;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        Debug.Log(playerConfigs.Length);
        for(int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerToSpawn, playerSpawns[i].position, playerSpawns[i].rotation);
        }
    }

}
