using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigManager : MonoBehaviour
{
    private List<PlayerConfig> playerConfigs;

    [SerializeField]
    private int maxPlayers = 2;

    public static PlayerConfigManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfig>();
        }
    }

    public void SetPlayerCharacter(int index, GameObject playerObject)
    {
        playerConfigs[index].playerObject = playerObject;
    }
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;
    }
    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex)) //If player not already added, add the player
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfig(pi));
        }
    }
}

public class PlayerConfig
{
    public PlayerConfig(PlayerInput pi)
    {
        playerIndex = pi.playerIndex;
        input = pi;
    }

    public PlayerInput input { get; set; }
    public int playerIndex { get; set; }
    public bool isReady { get; set; }
    public GameObject playerObject { get; set; }
}
