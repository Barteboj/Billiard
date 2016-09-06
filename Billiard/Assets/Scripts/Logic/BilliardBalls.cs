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