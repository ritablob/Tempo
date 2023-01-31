using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] DynamicCamera camScript;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();
        Destroy(GameObject.FindGameObjectWithTag("Manager"));
        for(int i = 0; i < playerConfigs.Length; i++)
        {
            var player = PlayerInput.Instantiate(playerConfigs[i].CharacterData.characterPrefab, i, controlScheme: playerConfigs[i].Input.currentControlScheme, -1, pairWithDevice: playerConfigs[i].Device);
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.transform.position = playerSpawns[i].position;
            player.GetComponent<PlayerMovement>().playerIndex = i;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            camScript.AddPlayer();
        }
    }

}
