using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public GameObject choosePlayersNamesMenu;
    public GameObject mainMenu;
    public Text playerOneText;
    public Text playerTwoText;

    public void ViewChoosePlayersNamesMenu()
    {
        mainMenu.SetActive(false);
        choosePlayersNamesMenu.SetActive(true);
    }

    public void ViewMainMenu()
    {
        mainMenu.SetActive(true);
        choosePlayersNamesMenu.SetActive(false);
    }

    public void StartGame()
    {
        Nicknames.player1Name = playerOneText.text;
        Nicknames.player2Name = playerTwoText.text;
        SceneManager.LoadScene("Billiard room");
    }

    public void Quit()
    {
        Application.Quit();
    }
}