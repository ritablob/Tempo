using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    [SerializeField] public List<PlayerConfig> playerConfigs;

    [SerializeField] public int maxPlayers = 2;
    [SerializeField] GameObject selectionScreenPrefab;
    [SerializeField] GameObject[] spawnPositions;
    public int readyPlayers = 0;
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
    public void SetPlayerCharacter(GameObject character, int index, CharacterSelection selection)
    {
        playerConfigs[index].character = character;
        //Debug.Log(playerConfigs[index].character.name + "     " + index);
        playerConfigs[index].scriptable = selection;
    }
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        readyPlayers++;

        if (readyPlayers == maxPlayers)
        {
            readyIcon.sprite = readySprites[1];
            SceneManager.LoadScene("Arena", LoadSceneMode.Single);
            // set ready to pink
        }
    }
    public void UnReadyPlayer(int index)
    {
        readyPlayers--;
        readyIcon.sprite = readySprites[0];
        playerConfigs[index].IsReady = false;
    }
    public void HandlePlayerJoin(PlayerInput pi) //Spawn player's char select
    {
        pi.transform.SetParent(spawnPositions[pi.playerIndex].transform);
        pi.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        playerConfigs.Add(new PlayerConfig(pi)); //Adds player config so we can store their input data
    }

    public void SpawnSelectionMenu(Player_Menu _playerMenuObject)
    {
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        bool canSpawn = true;

        foreach(PlayerInput input in players)
        {
            if(input.devices[0] == _playerMenuObject.GetComponent<PlayerInput>().devices[0]) { canSpawn = false; }
        }

        if (!canSpawn) { return; }

        Vector3 empty = new Vector3(0, 0, 0);

        selection1 = Instantiate(selectionScreenPrefab, empty, spawnPositions[playerConfigs.Count - 1].transform.rotation);
        _playerMenuObject.playerID = playerConfigs.Count - 1;
        return;
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
    public GameObject character { get; set; }
    public CharacterSelection scriptable { get; set; }
}
