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
    public TextMeshProUGUI playerText;
    public Image winnerImage;
    [SerializeField] DynamicCamera camera;
    public Pointer pointer;
    public CharacterSelection[] characters;
    public Button exitButton;

    private PlayerIngameMenu[] playerIngameMenus;

    public void WinScreen()
    {
        winShtufish.SetActive(true);
        if (playerIndex == 0)
        {
            playerText.text = "Player 2";
            for (int i = 0; i < characters.Length; i++)
            {
                string instanceName = characters[i].characterPrefab.name + "(Clone)";
                if (camera.player2.gameObject.name == instanceName)
                {
                    winnerText.text = characters[i].nameText;
                    winnerImage.sprite = characters[i].characterImageSprite;
                }
            }
            // player 2 wins
        }
        else
        {
            playerText.text = "Player 1";
            for (int i = 0; i < characters.Length; i++)
            {
                string instanceName = characters[i].characterPrefab.name + "(Clone)";
                if (camera.player1.gameObject.name == instanceName)
                {
                    winnerText.text = characters[i].nameText;
                    winnerImage.sprite = characters[i].characterImageSprite;
                }
            }
            // player 1 wins
        }
        // all the stuff that needs to happen once win condition is met
    }
    public void QuitButton()
    {
        Debug.Log("quit");
        SceneManager.LoadScene(0);
    }
    public void RestartButton()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnEnable()
    {
        playerIngameMenus = FindObjectsOfType<PlayerIngameMenu>();
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(exitButton.gameObject);
        for (int i = 0; i < playerIngameMenus.Length; i++)
        {
            playerIngameMenus[i].WinSetup();
        }
    }
}
