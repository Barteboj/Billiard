using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private BilliardBall[] billiardBalls;
    private MeshRenderer[] ballsMeshRenderers;

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

    void Awake()
    {
        billiardBalls = new BilliardBall[balls.Length];
        ballsMeshRenderers = new MeshRenderer[balls.Length];
        for (int i = 0; i < billiardBalls.Length; ++i)
        {
            billiardBalls[i] = balls[i].GetComponent<BilliardBall>();
            ballsMeshRenderers[i] = balls[i].GetComponent<MeshRenderer>();
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
                if (ballsMeshRenderers[i].enabled)
                {
                    for (int j = i + 1; j < balls.Length; ++j)
                    {
                        if (ballsMeshRenderers[j].enabled)
                        {
                            if (billiardBalls[i].Collide(balls[j]))
                            {
                                numberOfCollisions++;
                            }
                        }
                    }
                }
            }
        } while (numberOfCollisions > 0);
    }

    //in 8-pool balls in bottom corners of triangle must be of different type, ball in center is ball 8, rest of balls are random
    public void SetInTriangle(Vector3 positionOfFirstBall)
    {
        List<GameObject> ballsLeftToSet = new List<GameObject>(balls);
        ballsLeftToSet.RemoveAt(15);
        int randomLeftCornerBallIndex = Random.Range(0, 2) == 0 ? Random.Range(0, 7) : Random.Range(8, 15);
        int randomRightCornerBallIndex = randomLeftCornerBallIndex > 6 ? Random.Range(0, 7) : Random.Range(8, 15);
        ballsLeftToSet[randomLeftCornerBallIndex].transform.position = new Vector3(positionOfFirstBall.x - ((balls[0].GetComponent<BilliardBall>().Radius) * 4), balls[0].transform.position.y, positionOfFirstBall.z + (4 * (Mathf.Sqrt(3) / 2) * (balls[0].GetComponent<BilliardBall>().Radius * 2)));
        ballsLeftToSet[randomRightCornerBallIndex].transform.position = new Vector3(positionOfFirstBall.x - ((balls[0].GetComponent<BilliardBall>().Radius) * 4) + 4 * (balls[0].GetComponent<BilliardBall>().Radius * 2), balls[0].transform.position.y, positionOfFirstBall.z + (4 * (Mathf.Sqrt(3) / 2) * (balls[0].GetComponent<BilliardBall>().Radius * 2)));
        ballsLeftToSet[7].transform.position = new Vector3(positionOfFirstBall.x, positionOfFirstBall.y, positionOfFirstBall.z + 2 * (Mathf.Sqrt(3) / 2) * (balls[0].GetComponent<BilliardBall>().Radius * 2));
        ballsLeftToSet.RemoveAt(randomLeftCornerBallIndex);
        ballsLeftToSet.RemoveAt(randomLeftCornerBallIndex > 6 ? randomRightCornerBallIndex : randomRightCornerBallIndex - 1);
        ballsLeftToSet.RemoveAt(6);

        int actualBall = Random.Range(0, 12);
        ballsLeftToSet[actualBall].transform.position = positionOfFirstBall;
        ballsLeftToSet.RemoveAt(actualBall);
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 0; j <= i; ++j)
            {
                if ((i == 2 && j == 1) || (i == 4 && (j == 0 || j == 4)))
                {
                    continue;
                }
                actualBall = Random.Range(0, ballsLeftToSet.Count);
                ballsLeftToSet[actualBall].transform.position = new Vector3(positionOfFirstBall.x - ((balls[actualBall].GetComponent<BilliardBall>().Radius) * i) + j * (balls[actualBall].GetComponent<BilliardBall>().Radius * 2), balls[actualBall].transform.position.y, positionOfFirstBall.z + (i * (Mathf.Sqrt(3) / 2) * (balls[actualBall].GetComponent<BilliardBall>().Radius * 2)));
                ballsLeftToSet.RemoveAt(actualBall);
            }
        }
    }

    void Start()
    {
        SetInTriangle(new Vector3(0, 0.917f, 0.25f));
    }

    void Update()
    {
        HandleCollisions();
    }
}