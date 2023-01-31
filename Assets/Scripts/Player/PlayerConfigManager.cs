using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerConfigManager : MonoBehaviour
{
    [SerializeField] private List<PlayerConfig> playerConfigs;

    [SerializeField] private int maxPlayers = 2;
    [SerializeField] GameObject selectionScreenPrefab;
    [SerializeField] GameObject[] spawnPositions;
    public Image readyIcon;
    public Sprite[] readySprites;

    private GameObject selection1, selection2;

    public static PlayerConfigManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfig>();
            Cursor.visible = false;
        }
    }

    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
    }
    public void SetPlayerCharacter(CharacterSelection character, int index)
    {
        playerConfigs[index].CharacterData = character;
    }
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            readyIcon.sprite = readySprites[1];
            // set ready to pink

        }
        
    }
    public void UnReadyPlayer(int index)
    {
        readyIcon.sprite = readySprites[0];
        playerConfigs[index].IsReady = false;
    }
    public void HandlePlayerJoin(PlayerInput pi) //Spawn player's char select
    {
        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex)) //If player not already added, add the player
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfig(pi)); //Adds player config so we can store their input data
        }
    }

    public void SpawnSelectionMenu(Player_Menu _playerMenuObject)
    {
        if(playerConfigs.Count == 1)
        {
            selection1 = Instantiate(selectionScreenPrefab, spawnPositions[0].transform.position, spawnPositions[0].transform.rotation);
            _playerMenuObject.playerID = 0;
            return;
        }
        if(playerConfigs.Count == 0)
        {
            selection2 = Instantiate(selectionScreenPrefab, spawnPositions[1].transform.position, spawnPositions[1].transform.rotation);
            _playerMenuObject.playerID = 1;
        }
    }
}

public class PlayerConfig
{
    public PlayerConfig(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
        Device = Input.user.pairedDevices[0];
    }

    public PlayerInput Input { get; set; }
    public InputDevice Device { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public CharacterSelection CharacterData { get; set; }
}
