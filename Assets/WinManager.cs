using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public int playerIndex;
    public GameObject winShtufish;
    public TextMeshProUGUI winnerText;
    public Image winnerImage;
    [SerializeField] DynamicCamera camera;
    public CharacterSelection[] characters;

    public void WinScreen()
    {
        winShtufish.SetActive(true);
        if (playerIndex == 0)
        {
            winnerText.text = "Player 1";
            for (int i = 0; i < characters.Length; i++)
            {
                string instanceName = characters[i].characterPrefab.name + "(Clone)";
                if (camera.player1.gameObject.name == instanceName)
                {
                    winnerImage.sprite = characters[i].characterImageSprite;
                }
            }
            // player 1
        }
        else
        {
            winnerText.text = "Player 2";
            for (int i = 0; i < characters.Length; i++)
            {
                string instanceName = characters[i].characterPrefab.name + "(Clone)";
                if (camera.player2.gameObject.name == instanceName)
                {
                    winnerImage.sprite = characters[i].characterImageSprite;
                }
            }
            // player 2
        }
        // all the stuff that needs to happen once win condition is met
    }
    public void QuitButton()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
