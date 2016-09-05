using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGUIController : MonoBehaviour
{
    public GameObject[] ballsPlaceHolders;
    public GameObject ballTypePlaceHolder;
    public Text playerNameText;
    public BilliardBalls billiardBalls;

    public void ViewPlayerActivity(bool isActive)
    {
        playerNameText.color = isActive ? Color.yellow : Color.white;
    }

    public void ViewPocketedBall(int ballNumber)
    {
        foreach (GameObject ballPlaceholder in ballsPlaceHolders)
        {
            Image ballPlaceholderImage = ballPlaceholder.GetComponent<Image>();
            if (!ballPlaceholderImage.enabled)
            {
                ballPlaceholderImage.sprite = billiardBalls.ballsSprites[ballNumber];
                ballPlaceholderImage.enabled = true;
                return;
            }
        }
    }

    public void ViewBallType(BilliardBalls.BallType ballType)
    {
        Image ballTypePlaceHolderImage = ballTypePlaceHolder.GetComponent<Image>();
        switch (ballType)
        {
            case BilliardBalls.BallType.Full:
                ballTypePlaceHolderImage.sprite = billiardBalls.fullBallSprite;
                break;
            case BilliardBalls.BallType.Half:
                ballTypePlaceHolderImage.sprite = billiardBalls.halfBallSprite;
                break;
            case BilliardBalls.BallType.FullOrHalf:
                ballTypePlaceHolderImage.sprite = billiardBalls.fullOrHalfBallSprite;
                break;
        }
        ballTypePlaceHolderImage.enabled = true;
    }
}