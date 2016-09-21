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
    public CameraController cameraController;
    public FireworksEmitter fireworksEmitter;
    public Transform billiardTableTransform;
    public PlayerInputController playerInputController;

    public void ShowEndGameGUI(int winnerIndex)
    {
        playerInputController.enabled = false;
        fireworksEmitter.enabled = true;
        cameraController.topDownHeight = 2.3f;
        cameraController.topDownPosition = new Vector2(billiardTableTransform.position.x, billiardTableTransform.position.z);
        cameraController.SetCameraState(CameraState.TOPDOWN);
        cameraController.enabled = false;
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