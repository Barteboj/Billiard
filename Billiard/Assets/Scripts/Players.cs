using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class Players : MonoBehaviour
{
    private string[] playerName;
    [SerializeField]
    private List<int> player1Balls;
    [SerializeField]
    private List<int> player2Balls;
    private int[] ballsLeft;
    public bool[] balls;
    private int[] playerColor;
    [SerializeField]
    private int activePlayer;
    private bool stickUsed;
    private bool whiteBilliardBall;
    private GameObject[] playerPanels;
    public GameObject player1PanelText;
    public GameObject player2PanelText;
    public GameObject player1PanelBalls;
    public GameObject player2PanelBalls;
    private Text text;
    private bool endMove;
    public Text player1ColorText;
    public Text player2ColorText;
    public GameObject[] playerParticleEffect;

    public bool StickUsed
    {
        get
        {
            return stickUsed;
        }
        set
        {
            stickUsed = value;
        }
    }

    public bool EndMove
    {
        get
        {
            return endMove;
        }
        set
        {
            endMove = value;
        }
    }

    public bool WhiteBilliardBall
    {
        get
        {
            return whiteBilliardBall;
        }
        set
        {
            whiteBilliardBall = value;
        }
    }

    private void InitializePlayers()
    {
        playerName = new string[2];
        playerName[0] = Nicknames.player1Name;
        playerName[1] = Nicknames.player2Name;

        player1Balls = new List<int>();
        player2Balls = new List<int>();
        ballsLeft = new int[2];
        ballsLeft[0] = 7;
        ballsLeft[0] = 7;
        playerColor = new int[2];
        playerColor[0] = 0;
        playerColor[1] = 0;
        activePlayer = 0;
        stickUsed = false;
        whiteBilliardBall = false;
        playerPanels = GameObject.FindGameObjectsWithTag("Panel");
        playerPanels[0].GetComponent<Image>().color = Color.yellow;
        playerPanels[1].GetComponent<Image>().color = Color.white;
        playerParticleEffect[0].SetActive(true);
        text = player1PanelText.GetComponent<Text>();
        text.text = playerName[0];
        text = player2PanelText.GetComponent<Text>();
        text.text = playerName[1];
        balls = new bool[16];
    }


    public void AddBilliardBall(int billiardBallNumber)
    {
        Debug.Log(billiardBallNumber);
        if(activePlayer == 0)
            player1Balls.Add(billiardBallNumber);
        else
            player2Balls.Add(billiardBallNumber);
        ballsLeft[activePlayer]--;
        ShowBalls();
    }

    public void WrongBillardBall()
    {
        ballsLeft[activePlayer]--;
    }

    public bool IsAllBilliardBallsPocketed()
    {
        if (ballsLeft[activePlayer] == 0)
            return true;
        else
            return false;
    }

    public bool IsBilliardBallsPocketed(int billiardBallNumber)
    {
        foreach (int ball in player1Balls)
        {
            if (ball == billiardBallNumber)
                return true;
        }

        foreach(int ball in player2Balls)
        {
            if (ball == billiardBallNumber)
                return true;
        }

        return false;
    }

    public bool CheckBilliardBallsColor(int number)
    {
       
        if(playerColor[activePlayer] == 0)
        {
            SetBilliardBallsColor(number);
            return true;
        }
        else
        {
            if(playerColor[activePlayer] == ChangeColorToInt(number))
                return true;
            else
                return false;
        }
    }

    public void SetBilliardBallsColor(int number)
    {
        if(activePlayer == 0)
        {
            if(number < 8)
            {
                playerColor[0] = 1;
                player1ColorText.text = "Całe";
                playerColor[1] = 2;
                player2ColorText.text = "Połowki";
            }
            else
            {
                playerColor[0] = 2;
                player1ColorText.text = "Połowki";
                playerColor[1] = 1;
                player2ColorText.text = "Całe";
            }
        }
        else 
        {
            if (number < 8)
            {
                playerColor[1] = 1;
                player2ColorText.text = "Całe";
                playerColor[0] = 2;
                player1ColorText.text = "Połowki";
            }
            else
            {
                playerColor[1] = 2;
                player1ColorText.text = "Połowki";
                playerColor[0] = 1;
                player2ColorText.text = "Całe";
            }
        }
    }

    public int ChangeColorToInt(int number)
    {
        return (number < 8) ? 1 : 2;
    }

    public void ChangePlayer()
    {
        playerPanels[activePlayer].GetComponent<Image>().color = Color.white;
        playerParticleEffect[activePlayer].SetActive(false);
        activePlayer = (activePlayer == 0) ? 1 : 0;
        playerPanels[activePlayer].GetComponent<Image>().color = Color.yellow;
        playerParticleEffect[activePlayer].SetActive(true);
    }

    public void SetWhiteBilliardBall(GameObject billardBall)
    {
        billardBall.transform.position = new Vector3(0, 0.917f, -1.106f);
    }

    public void ShowBalls()
    {
        StringBuilder sb = new StringBuilder();
        foreach(int ball in player1Balls)
        {
            sb.Append(ball);
            sb.Append(" ");
        }
        text = player1PanelBalls.GetComponent<Text>();
        text.text = sb.ToString();
        sb.Remove(0, sb.Length);

        foreach (int ball in player2Balls)
        {
            sb.Append(ball);
            sb.Append(" ");
        }
        text = player2PanelBalls.GetComponent<Text>();
        text.text = sb.ToString();
        sb.Remove(0, sb.Length);
    }

    public void setBalls()
    {
        for (int i = 0; i < 16; i++)
            balls[i] = false;
    }

    public bool checkBalls()
    {
        bool result = false;
        for (int i = 0; i < 16; i++)
            if (balls[i] != false)
                result = true;
        return result;
    }

    

    // Use this for initialization
    void Start () {
        InitializePlayers();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
