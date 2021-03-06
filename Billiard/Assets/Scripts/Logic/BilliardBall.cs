﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BilliardBall : MonoBehaviour
{
    //0 means white ball
    [SerializeField]
    private int number;
    [SerializeField]
    private float mass;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Vector3 speed;
    private float friction;
    private float speedDownLimit;
    private GameObject[] holes;
    private GameObject[] borders;
    public Players players;
    [SerializeField]
    private bool pocketed;
    private AudioSource billiardBallAudioSource;
    public AudioSource billiardBallHitsRailAudioSource;
    public AudioSource billiardBallPocketedAudioSource;

    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
        }
    }

    public float Mass
    {
        get
        {
            return mass;
        }
        set
        {
            mass = value;
        }
    }

    public float Radius
    {
        get
        {
            return radius;
        }
        set
        {
            radius = value;
        }
    }

    public Vector3 Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public float Friction
    {
        get
        {
            return friction;
        }
        set
        {
            friction = value;
        }
    }

    public float SpeedDownLimit
    {
        get
        {
            return speedDownLimit;
        }
        set
        {
            speedDownLimit = value;
        }
    }

    public bool Pocketed
    {
        get
        {
            return pocketed;
        }
        set
        {
            pocketed = value;
        }
    }

    public BilliardBall()
    {
        friction = 0.2f;
        speedDownLimit = 0.02f;
    }

    void Awake()
    {
        billiardBallAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Accelarate()
    {
        Vector3 previousSpeed = speed;

        if (previousSpeed != Vector3.zero)
        {
            if (previousSpeed.x > 0)
            {
                speed.x -= friction * Mathf.Abs(speed.x / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }
            else
            {
                speed.x += friction * Mathf.Abs(speed.x / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }

            if (previousSpeed.z > 0)
            {
                speed.z -= friction * Mathf.Abs(speed.z / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }
            else
            {
                speed.z += friction * Mathf.Abs(speed.z / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }
        }
        if (Mathf.Sqrt(Mathf.Pow(speed.x, 2) + Mathf.Pow(speed.z, 2)) < speedDownLimit)
        {
            speed = new Vector3(0, 0, 0);
        }
    }

    public void Roll()
    {
        Vector3 appliedRotation;

        Vector3 appliedShift = speed * Time.deltaTime;

        gameObject.transform.position += appliedShift;

        appliedRotation.x = appliedShift.z / (2 * Mathf.PI * radius) * 360;
        appliedRotation.y = 0;
        appliedRotation.z = -appliedShift.x / (2 * Mathf.PI * radius) * 360;

        gameObject.transform.Rotate(appliedRotation.x, appliedRotation.y, appliedRotation.z, Space.World);
    }

    public void Roll(Vector3 shift)
    {
        Vector3 rotation;

        gameObject.transform.position += shift;

        rotation.x = shift.z / (2 * Mathf.PI * radius) * 360;
        rotation.y = 0;
        rotation.z = -shift.x / (2 * Mathf.PI * radius) * 360;

        gameObject.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.World);
    }

    public void Collide()
    {
        foreach (GameObject border in borders)
        {
            Vector3 borderEulerAngles = border.transform.eulerAngles;
            Vector3 billiardBallPosition = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            Vector3 borderPosition = new Vector3(border.transform.position.x, 0, border.transform.position.z);
            Vector3 distanceVector = billiardBallPosition - borderPosition;
            float distance = distanceVector.magnitude;
            if (borderEulerAngles.y > 300)
            {
                if (distance < border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius && ((gameObject.transform.position.z > border.transform.position.z && border.transform.position.x < 0) || (gameObject.transform.position.x > border.transform.position.x && border.transform.position.x > 0)))
                {
                    Roll(new Vector3(((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2, 0, ((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2));
                    speed = new Vector3(-speed.z, speed.y, -speed.x);
                    billiardBallHitsRailAudioSource.Play();
                }
            }
            else if (borderEulerAngles.y > 40 && borderEulerAngles.y < 50)
            {
                if (distance < border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius && ((gameObject.transform.position.z < border.transform.position.z && border.transform.position.x < 0) || (gameObject.transform.position.x > border.transform.position.x && border.transform.position.x > 0)))
                {
                    Roll(new Vector3(((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2, 0, -((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2));
                    speed = new Vector3(speed.z, speed.y, speed.x);
                    billiardBallHitsRailAudioSource.Play();
                }
            }
            else if (borderEulerAngles.y > 130 && borderEulerAngles.y < 140)
            {
                if (distance < border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius && ((gameObject.transform.position.x < border.transform.position.x && border.transform.position.x < 0) || (gameObject.transform.position.z < border.transform.position.z && border.transform.position.x > 0)))
                {
                    Roll(new Vector3(-((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2, 0, -((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2));
                    speed = new Vector3(-speed.z, speed.y, -speed.x);
                    billiardBallHitsRailAudioSource.Play();
                }
            }
            else if (borderEulerAngles.y > 220 && borderEulerAngles.y < 230)
            {
                if (distance < border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius && ((gameObject.transform.position.z > border.transform.position.z && border.transform.position.x > 0) || (gameObject.transform.position.x < border.transform.position.x && border.transform.position.x < 0)))
                {
                    Roll(new Vector3(-((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2, 0, ((border.transform.localScale.x * Mathf.Sqrt(2) / 2 + radius) - distance) * Mathf.Sqrt(2) / 2));
                    speed = new Vector3(speed.z, speed.y, speed.x);
                    billiardBallHitsRailAudioSource.Play();
                }
            }
            else if (Mathf.Sqrt(Mathf.Pow(border.transform.localScale.z / 2, 2) + Mathf.Pow(border.transform.localScale.x / 2, 2)) + radius > distance)
            {
                if (border.transform.rotation.y == 0)
                {
                    if ((gameObject.transform.position.z > border.transform.position.z - border.transform.localScale.z / 2 && gameObject.transform.position.z < border.transform.position.z + border.transform.localScale.z / 2) && Mathf.Abs(gameObject.transform.position.x) >= Mathf.Abs(border.transform.position.x) - border.transform.localScale.x / 2 - radius)
                    {
                        if (gameObject.transform.position.x > 0 && border.transform.position.x > 0)
                        {
                            Roll(new Vector3(border.transform.position.x - border.transform.localScale.x / 2 - radius - gameObject.transform.position.x, 0, 0));
                            speed = new Vector3(-speed.x, speed.y, speed.z);
                            billiardBallHitsRailAudioSource.Play();
                            return;
                        }
                        else if (gameObject.transform.position.x < 0 && border.transform.position.x < 0)
                        {
                            Roll(new Vector3(border.transform.position.x + border.transform.localScale.x / 2 + radius - gameObject.transform.position.x, 0, 0));
                            speed = new Vector3(-speed.x, speed.y, speed.z);
                            billiardBallHitsRailAudioSource.Play();
                            return;
                        }
                    }
                }
                else
                {
                    if ((gameObject.transform.position.x > border.transform.position.x - border.transform.localScale.z / 2 && gameObject.transform.position.x < border.transform.position.x + border.transform.localScale.z / 2) && Mathf.Abs(gameObject.transform.position.z) >= Mathf.Abs(border.transform.position.z) - border.transform.localScale.x / 2 - radius)
                    {
                        if (gameObject.transform.position.z > 0 && border.transform.position.z > 0)
                        {
                            Roll(new Vector3(0, 0, border.transform.position.z - border.transform.localScale.x / 2 - radius - gameObject.transform.position.z));
                            speed = new Vector3(speed.x, speed.y, -speed.z);
                            billiardBallHitsRailAudioSource.Play();
                            return;
                        }
                        else if (gameObject.transform.position.z < 0 && border.transform.position.z < 0)
                        {
                            Roll(new Vector3(0, 0, border.transform.position.z + border.transform.localScale.x / 2 + radius - gameObject.transform.position.z));
                            speed = new Vector3(speed.x, speed.y, -speed.z);
                            billiardBallHitsRailAudioSource.Play();
                            return;
                        }
                    }
                }
            }
        }
        foreach (GameObject hole in holes)
        {
            hole.transform.position = new Vector3(hole.transform.position.x, gameObject.transform.position.y, hole.transform.position.z);
            Vector3 offset = hole.transform.position - gameObject.transform.position;
            if (!pocketed && (offset.magnitude < 0.5f * hole.transform.localScale.x))
            {
                billiardBallPocketedAudioSource.Play();
                if (this.number == 0)
                {
                    players.wasWhiteBallPocketedInActualTurn = true;
                    FindObjectOfType<MessagesController>().ShowMessage(players.GetActivePlayerName() + " pocketed white ball");
                    FindObjectOfType<Players>().WasFoul = true;
                    players.StickUsed = false;
                    players.WhiteBilliardBall = true;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                else if (this.number == 8)
                {
                    if (players.IsAllBilliardBallsPocketed())
                    {
                        players.isCheckingIfWhiteBallWillBePocketedWithEightBall = true;
                    }
                    else
                    {
                        FindObjectOfType<EndGameGUIController>().ShowEndGameGUI(players.ActivePlayerIndex == 0 ? 1 : 0);
                    }
                    pocketed = true;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    billiardBallPocketedAudioSource.Play();
                }
                else
                {
                    if (!players.CheckBilliardBallsColor(this.number))
                    {
                        FindObjectOfType<Players>().WasFoul = true;
                    }
                    FindObjectOfType<MessagesController>().ShowMessage(players.GetActivePlayerName() + " pocketed ball number " + number);
                    players.AddBilliardBall(this.number);
                    pocketed = true;
                    players.StickUsed = false;
                    players.balls[number] = true;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                gameObject.GetComponent<BilliardBall>().speed = new Vector3(0, 0, 0);
            }
        }
    }

    public bool CheckCollision(GameObject collidingObject)
    {
        if (collidingObject.GetComponent<BilliardBall>())
        {
            float distanceBetweenObjects = Vector3.Distance(gameObject.transform.position, collidingObject.transform.position);
            float radiusesSum = gameObject.GetComponent<BilliardBall>().radius + collidingObject.GetComponent<BilliardBall>().radius;
            if (distanceBetweenObjects < radiusesSum - 0.00001 && collidingObject.GetComponent<MeshRenderer>().enabled) //- 0.00001 to prevent collisions in triangle
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool Collide(GameObject collidingObject)
    {
        if (CheckCollision(collidingObject))
        {
            ReactOnCollision(collidingObject);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReactOnCollision(GameObject collidingObject)
    {
        float distanceBetweenObjects = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - collidingObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z - collidingObject.transform.position.z, 2));
        float radiusesSum = gameObject.GetComponent<BilliardBall>().radius + collidingObject.GetComponent<BilliardBall>().radius;
        float collisionPower = (collidingObject.GetComponent<BilliardBall>().speed - speed).magnitude;

        BilliardBall collidingObjectComponent = collidingObject.GetComponent<BilliardBall>();
        billiardBallAudioSource.volume = Mathf.Clamp01(collisionPower / 4f);
        billiardBallAudioSource.Play();

        //when balls overlapping each other than push them aside
        if (distanceBetweenObjects < radiusesSum)
        {
            Vector3 vectorBetweenBalls = new Vector3(collidingObject.transform.position.x - gameObject.transform.position.x, 0, collidingObject.transform.position.z - gameObject.transform.position.z);
            Vector3 overlapVector = vectorBetweenBalls.normalized * radius * 2 - vectorBetweenBalls;
            if (gameObject.transform.position.x >= collidingObject.transform.position.x)
            {
                Roll(new Vector3(Mathf.Abs(overlapVector.x / 2), 0, 0));
                collidingObjectComponent.Roll(new Vector3(-Mathf.Abs(overlapVector.x / 2), 0, 0));
            }
            else
            {
                Roll(new Vector3(-Mathf.Abs(overlapVector.x), 0, 0));
                collidingObjectComponent.Roll(new Vector3(Mathf.Abs(overlapVector.x / 2), 0, 0));
            }

            if (gameObject.transform.position.z >= collidingObject.transform.position.z)
            {
                Roll(new Vector3(0, 0, Mathf.Abs(overlapVector.z / 2)));
                collidingObjectComponent.Roll(new Vector3(0, 0, -Mathf.Abs(overlapVector.z / 2)));
            }
            else
            {
                Roll(new Vector3(0, 0, -Mathf.Abs(overlapVector.z / 2)));
                collidingObjectComponent.Roll(new Vector3(0, 0, Mathf.Abs(overlapVector.z / 2)));
            }
        }

        float dx = collidingObject.transform.position.x - gameObject.transform.position.x;
        float dz = collidingObject.transform.position.z - gameObject.transform.position.z;
        float d_kw = dx * dx + dz * dz;
        float lambda = (2 * dx * (collidingObject.GetComponent<BilliardBall>().speed.x - speed.x) + 2 * dz * (collidingObject.GetComponent<BilliardBall>().speed.z - speed.z)) / (d_kw + d_kw);

        speed.x += lambda * dx;
        speed.z += lambda * dz;
        collidingObject.GetComponent<BilliardBall>().speed.x -= lambda * dx;
        collidingObject.GetComponent<BilliardBall>().speed.z -= lambda * dz;
    }
    
    public void Move()
    {
        Accelarate();
        if (speed != Vector3.zero)
        {
            Collide();
            Roll(speed * Time.deltaTime);
        }
    }
    
    void Start()
    {
        holes = GameObject.FindGameObjectsWithTag("Hole");
        borders = GameObject.FindGameObjectsWithTag("Border");
        pocketed = false;
    }
    
    void Update()
    {
        Move();
    }
}