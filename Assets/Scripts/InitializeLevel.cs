using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private GameObject playerToSpawn;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        Destroy(GameObject.FindGameObjectWithTag("Manager"));
        for(int i = 0; i < playerConfigs.Length; i++)
        {
            var player = PlayerInput.Instantiate(playerConfigs[i].playerObject, i, controlScheme: playerConfigs[i].input.currentControlScheme, -1, pairWithDevice: playerConfigs[i].device);
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.transform.position = playerSpawns[i].position;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }

}
