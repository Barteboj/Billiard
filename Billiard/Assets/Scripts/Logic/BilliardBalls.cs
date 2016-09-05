using UnityEngine;
using System.Collections;

public class BilliardBalls : MonoBehaviour
{
    public enum BallType
    {
        Full,
        Half,
        FullOrHalf
    }

    [SerializeField]
    private GameObject[] balls;
    public Sprite[] ballsSprites;
    public Sprite fullBallSprite;
    public Sprite halfBallSprite;
    public Sprite fullOrHalfBallSprite;

    public GameObject[] Balls
    {
        get
        {
            return balls;
        }
        set
        {
            balls = value;
        }
    }

    public GameObject this[int index]
    {
        get
        {
            return balls[index];
        }
        set
        {
            balls[index] = value;
        }
    }


    public void HandleCollisions()
    {
        int numberOfCollisions;
        //repeat, because pushing balls aside can make new collisions
        do
        {
            numberOfCollisions = 0;
            for (int i = 0; i < balls.Length - 1; ++i)
            {
                BilliardBall ballIBilliardBall = balls[i].GetComponent<BilliardBall>();
                if (balls[i].GetComponent<MeshRenderer>().enabled)
                {
                    for (int j = i + 1; j < balls.Length; ++j)
                    {
                        if (balls[j].GetComponent<MeshRenderer>().enabled)
                        {
                            if (ballIBilliardBall.Collide(balls[j]))
                            {
                                numberOfCollisions++;
                            }
                        }
                    }
                }
            }
        } while (numberOfCollisions > 0);
    }

    //set balls in starting triangle position
    public void SetInTriangle(Vector3 positionOfFirstBall)
    {
        int actualBall = 0;
        balls[0].transform.position = positionOfFirstBall;
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 0; j <= i; ++j)
            {
                ++actualBall;
                balls[actualBall].transform.position = new Vector3(balls[0].transform.position.x - ((balls[actualBall].GetComponent<BilliardBall>().Radius) * i) + j * (balls[actualBall].GetComponent<BilliardBall>().Radius * 2), balls[actualBall].transform.position.y, balls[0].transform.position.z + (i * (Mathf.Sqrt(3) / 2) * (balls[actualBall].GetComponent<BilliardBall>().Radius * 2)));
                //balls[actualBall].transform.position = new Vector3(balls[actualBall - (j + 1)].transform.position.x - (balls[actualBall].GetComponent<BilliardBall>().radius * Mathf.Sqrt(2)) - ((i - (j + 1)) * (balls[actualBall].GetComponent<BilliardBall>().radius * Mathf.Sqrt(2))) + (j - (i - 1)) * (balls[actualBall].GetComponent<BilliardBall>().radius * Mathf.Sqrt(2)), balls[actualBall].transform.position.y, balls[0].transform.position.z + ((balls[0].GetComponent<BilliardBall>().radius * Mathf.Sqrt(2)) + (balls[0].GetComponent<BilliardBall>().radius * Mathf.Sqrt(2) * (i - 1))));
            }
        }

    }

    // Use this for initialization
    void Start()
    {
        SetInTriangle(new Vector3(0, 0.917f, 0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        HandleCollisions();
    }
}
