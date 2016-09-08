using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameGUIController : MonoBehaviour
{
    public GameObject endGameGUI;
    public GameObject playerOnePanel;
    public GameObject playerTwoPanel;
    public GameObject keysPanel;
    public GameObject billiardStick;
    public Text messagesText;
    public Text playerNameText;

    public void ShowEndGameGUI(int winnerIndex)
    {
        playerOnePanel.SetActive(false);
        playerTwoPanel.SetActive(false);
        keysPanel.SetActive(false);
        billiardStick.SetActive(false);
        messagesText.enabled = false;
        if (winnerIndex == 0)
        {
            playerNameText.text = Nicknames.player1Name;
        }
        else
        {
            playerNameText.text = Nicknames.player2Name;
        }
        endGameGUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Billiard room");
    }

    public void GetBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}