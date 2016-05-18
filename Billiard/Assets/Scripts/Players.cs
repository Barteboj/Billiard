using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Players : MonoBehaviour
{
    private string[] playerName;
    private List<int>[] playerBalls;
    private int[] ballsLeft;
    private int[] playerColor;
    [SerializeField]
    private int activePlayer;


    private void InitializePlayers()
    {
        playerName = new string[2];
        playerName[0] = "player1";
        playerName[1] = "player2";
        playerBalls = new List<int>[2];
        ballsLeft = new int[2];
        ballsLeft[0] = 7;
        ballsLeft[0] = 7;
        playerColor = new int[2];
        playerColor[0] = 0;
        playerColor[1] = 0;
        activePlayer = 0;
    }


    public void AddBilliardBall(int billiardBallNumber)
    {
        playerBalls[activePlayer].Add(billiardBallNumber);
        ballsLeft[activePlayer]--;
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
                playerColor[1] = 2;
            }
            else
            {
                playerColor[0] = 2;
                playerColor[1] = 1;
            }
        }
        else 
        {
            if (number < 8)
            {
                playerColor[1] = 1;
                playerColor[0] = 2;
            }
            else
            {
                playerColor[1] = 2;
                playerColor[0] = 1;
            }
        }
    }

    public int ChangeColorToInt(int number)
    {
        return (number < 8) ? 1 : 2;
    }

    public void ChangePlayer()
    {
        activePlayer = (activePlayer == 0) ? 1 : 0;
    }

    public void SetWhiteBilliardBall(GameObject billardBall)
    {
        billardBall.transform.position = new Vector3(0, 0.917f, -1.106f);
    }

    

    // Use this for initialization
    void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
