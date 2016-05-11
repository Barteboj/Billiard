using UnityEngine;
using System.Collections;

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

    public BilliardBall()
    {
        friction = 0.2f;
        speedDownLimit = 0.0009f;
    }

    //decelerate because of the friction
    public void Accelarate()
    {
        Vector3 previousSpeed = speed;

        if (previousSpeed != new Vector3(0, 0, 0))
        {
            if (previousSpeed.x > 0)
            {
                speed.x -= friction * Mathf.Abs(speed.x / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }
            else {
                speed.x += friction * Mathf.Abs(speed.x / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }

            if (previousSpeed.z > 0)
            {
                speed.z -= friction * Mathf.Abs(speed.z / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }
            else {
                speed.z += friction * Mathf.Abs(speed.z / (Mathf.Abs(speed.x) + Mathf.Abs(speed.z))) * Time.deltaTime;
            }
        }

        if (Mathf.Sqrt(Mathf.Pow(speed.x, 2) + Mathf.Pow(speed.z, 2)) < speedDownLimit)
        {
            speed = new Vector3(0, 0, 0);
        }
    }

    //ball rolling on the table
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

    //collisions with borders of table
    public void Collide()
    {
        if (gameObject.transform.position.x >= 0.5175f)
        {
            Roll(new Vector3(0.5175f - gameObject.transform.position.x, 0, 0));
            speed = new Vector3(-speed.x, speed.y, speed.z);
        }
        if (gameObject.transform.position.x <= -0.5175f)
        {
            Roll(new Vector3(-gameObject.transform.position.x - 0.5175f, 0, 0));
            speed = new Vector3(-speed.x, speed.y, speed.z);
        }

        if (gameObject.transform.position.z >= 0.795f)
        {
            Roll(new Vector3(0, 0, 0.795f - gameObject.transform.position.z));
            speed = new Vector3(speed.x, speed.y, -speed.z);
        }
        if (gameObject.transform.position.z <= -1.295f)
        {
            Roll(new Vector3(0, 0, -gameObject.transform.position.z - 1.295f));
            speed = new Vector3(speed.x, speed.y, -speed.z);
        }
    }

    public bool CheckCollision(GameObject collidingObject)
    {
        if (collidingObject.GetComponent<BilliardBall>())
        {
            float distanceBetweenObjects = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - collidingObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z - collidingObject.transform.position.z, 2));
            float radiusesSum = gameObject.GetComponent<BilliardBall>().radius + collidingObject.GetComponent<BilliardBall>().radius;
            if (distanceBetweenObjects < radiusesSum - 0.00001) //- 0.00001 to prevent collisions in triangle
            {
                return true;
            }
            else {
                return false;
            }
        }
        else {
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

        BilliardBall collidingObjectComponent = collidingObject.GetComponent<BilliardBall>();

        //when balls overlapping each other than push them aside
        if (distanceBetweenObjects < radiusesSum)
        {
            /*if ((speed != new Vector3(0, 0, 0) && collidingObject.GetComponent<BilliardBall>().speed != new Vector3(0, 0, 0)))
            {
                while (distanceBetweenObjects < radiusesSum)
                {
                    gameObject.transform.position -= speed / 10;
                    collidingObject.transform.position -= collidingObject.GetComponent<BilliardBall>().speed / 10;
                    distanceBetweenObjects = Mathf.Sqrt(Mathf.Pow(gameObject.transform.position.x - collidingObject.transform.position.x, 2) + Mathf.Pow(gameObject.transform.position.z - collidingObject.transform.position.z, 2));
                }
            }
            else
            {*/
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
            //}
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

    //standard ball move
    public void Move()
    {
        Accelarate();
        Collide();
        Roll(speed * Time.deltaTime);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
