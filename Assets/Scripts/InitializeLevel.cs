using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] DynamicCamera camScript;
    public List<PlayerUIController> uicontrollers;

    void Start()
    {
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();

        Destroy(GameObject.FindGameObjectWithTag("PLAYERCONFIGMAN"));

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = PlayerInput.Instantiate(playerConfigs[i].character, i, controlScheme: playerConfigs[i].Input.currentControlScheme, -1, pairWithDevice: playerConfigs[i].Device);
            uicontrollers[i].ChangeIcon(playerConfigs[i].scriptable.characterIconSprite);
            player.gameObject.GetComponent<CharacterController>().enabled = false;
            player.transform.position = playerSpawns[i].position;
            player.GetComponent<PlayerMovement>().playerIndex = i;
            player.gameObject.GetComponent<CharacterController>().enabled = true;
            camScript.AddPlayer();
        }
    }

}
