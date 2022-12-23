using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSetupMenu : MonoBehaviour
{
    int playerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
        titleText.SetText($"Player {index + 1} ");

    }

    void Update()
    {
        
    }

    public void SetCharacter(GameObject playerCharacter)
    {
        PlayerConfigManager.Instance.SetPlayerCharacter(playerIndex, playerCharacter);
    }
}
