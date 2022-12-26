using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerConfigManager : MonoBehaviour
{
    [SerializeField] private List<PlayerConfig> playerConfigs;

    [SerializeField] private int maxPlayers = 2;
    [SerializeField] GameObject menuSelectIconP1, menuSelectIconP2;
    [SerializeField] private GameObject[] playerCharacers;
    [SerializeField] Image p1CharPreview, p2CharPreview;
    [SerializeField] Animator anim;

    public GameObject[] charSelectPositions;

    private GameObject icon1, icon2;

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

    public List<PlayerConfig> GetPlayerConfigs()
    {
        return playerConfigs;
    }
    public void SetPlayerCharacter(int selectedPlayer, int index)
    {
        playerConfigs[index].playerObject = playerCharacers[selectedPlayer];
    }
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].isReady = true;
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady == true))
        {
            anim.SetTrigger("Ready");
        }
        
    }
    public void UnReadyPlayer(int index)
    {
        playerConfigs[index].isReady = false;
        anim.SetTrigger("UnReady");
    }
    public void HandlePlayerJoin(PlayerInput pi)
    {
        if(!playerConfigs.Any(p => p.playerIndex == pi.playerIndex)) //If player not already added, add the player
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfig(pi));
        }
    }

    public void SpawnPlayerIcon(Player_Menu _playerMenuObject)
    {
        if(playerConfigs.Count == 1)
        {
            icon1 = Instantiate(menuSelectIconP1, charSelectPositions[0].transform.position, charSelectPositions[0].transform.rotation);
            icon1.transform.parent = charSelectPositions[0].transform;
            _playerMenuObject.playerIcon = icon1;
            _playerMenuObject.preview = p1CharPreview;
            _playerMenuObject.playerID = 0;
        }
        else
        {
            icon2 = Instantiate(menuSelectIconP2, charSelectPositions[0].transform.position, charSelectPositions[0].transform.rotation);
            icon2.transform.parent = charSelectPositions[0].transform;
            _playerMenuObject.playerIcon = icon2;
            _playerMenuObject.preview = p2CharPreview;
            _playerMenuObject.playerID = 1;
        }
    }
}

public class PlayerConfig
{
    public PlayerConfig(PlayerInput pi)
    {
        playerIndex = pi.playerIndex;
        input = pi;
        device = input.user.pairedDevices[0];
    }

    public PlayerInput input { get; set; }
    public InputDevice device { get; set; }
    public int playerIndex { get; set; }
    public bool isReady { get; set; }
    public GameObject playerObject { get; set; }
}
