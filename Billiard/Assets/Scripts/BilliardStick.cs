using UnityEngine;
using System.Collections;

public enum ShotStage { PREPARINGSHOT, AIMING, POWERADJUSTING, SHOOTING, NOSHOT}

public class BilliardStick : MonoBehaviour {

    private float length;

    private float shotPower;

    private ShotStage shotStage;

    public void PrepareShot(GameObject billiardBall)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        gameObject.transform.position = new Vector3(billiardBall.transform.position.x, billiardBall.transform.position.y, billiardBall.transform.position.z - length - 0.06f);
        shotPower = 0;
        shotStage = ShotStage.AIMING;
    }

    public void Aim(GameObject billiardBall)
    {
        gameObject.transform.RotateAround(billiardBall.transform.position, new Vector3(0, 1, 0), -Input.GetAxis("Mouse X") * 2);
        if (Input.GetMouseButtonDown(0))
        {
            shotStage = ShotStage.POWERADJUSTING;
        }
    }

    public void AdjustPower()
    {
        if ((shotPower < -0.01 && Input.GetAxis("Mouse Y") >= 0) || (shotPower > -0.5f && Input.GetAxis("Mouse Y") <= 0))
        {
            float powerCorrection = 0;
            shotPower += Input.GetAxis("Mouse Y") / 30;
            if (shotPower > -0.01)
            {
                powerCorrection = shotPower + 0.01f;
                shotPower -= powerCorrection;
            }
            else if (shotPower < -0.5f)
            {
                powerCorrection = shotPower + 0.5f;
                shotPower -= powerCorrection;
            }
            gameObject.transform.Translate(0, 0, (Input.GetAxis("Mouse Y") / 30) - powerCorrection);
        }
        if (Input.GetMouseButtonUp(0))
        {
            shotStage = ShotStage.SHOOTING;
        }
    }

    public void Shot(GameObject billiardBall)
    {
        Vector3 previousBilliardStickPosition = gameObject.transform.position;
        gameObject.transform.Translate(0, 0, -shotPower * 5 * Time.deltaTime);
        if (Vector3.Distance(gameObject.transform.position, billiardBall.transform.position) < length + 0.03)
        {
            billiardBall.GetComponent<BilliardBall>().Speed = (gameObject.transform.position - previousBilliardStickPosition) / Time.deltaTime;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            shotStage = ShotStage.NOSHOT;
        }
    }

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        shotPower = 0.01f;
        shotStage = ShotStage.NOSHOT;
        length = 1.48f;
        PrepareShot(GameObject.Find("White Ball"));
	}
	
	// Update is called once per frame
	void Update () {
        switch (shotStage)
        {
            case ShotStage.PREPARINGSHOT:
                PrepareShot(GameObject.Find("White Ball"));
                break;
            case ShotStage.AIMING:
                Aim(GameObject.Find("White Ball"));
                break;
            case ShotStage.POWERADJUSTING:
                AdjustPower();
                break;
            case ShotStage.SHOOTING:
                Shot(GameObject.Find("White Ball"));
                break;
            case ShotStage.NOSHOT:
                int numberOfMovingBalls = 0;
                for (int i = 0; i < 16; ++i)
                {
                    if (GameObject.Find("Manager").GetComponent<BilliardBalls>()[i].GetComponent<BilliardBall>().Speed != new Vector3(0, 0, 0))
                    {
                        ++numberOfMovingBalls;
                    }
                }
                if (numberOfMovingBalls == 0)
                {
                    shotStage = ShotStage.PREPARINGSHOT;
                }
                break;
        }
	}
}
