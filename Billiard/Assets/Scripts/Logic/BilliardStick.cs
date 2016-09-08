using UnityEngine;
using System.Collections;

public enum ShotStage
{
    PREPARINGSHOT,
    AIMING,
    POWERADJUSTING,
    SHOOTING,
    NOSHOT
}

public class BilliardStick : MonoBehaviour {

    private float length;

    private float shotPower;

    private ShotStage shotStage;

    private bool isViewfinderEnabled = true;

    public Players players;

    public GameObject viewfinder;
    public AudioSource stickHitBallAudioSource;

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
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - gameObject.transform.position.y;
        Vector3 posToLookAt = Camera.main.ScreenToWorldPoint(mousePos);
        posToLookAt.y = gameObject.transform.position.y;
        gameObject.transform.position = billiardBall.transform.position;
        gameObject.transform.LookAt(posToLookAt);
        gameObject.transform.Translate(0, 0, -(length + billiardBall.GetComponent<BilliardBall>().Radius + 0.05f));
        if (isViewfinderEnabled)
        {
            viewfinder.SetActive(true);
        }
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
            viewfinder.SetActive(false);
            shotStage = ShotStage.SHOOTING;
        }
    }

    public void Shot(GameObject billiardBall)
    {
        Vector3 previousBilliardStickPosition = gameObject.transform.position;
        gameObject.transform.Translate(0, 0, -shotPower * 5 * Time.deltaTime);
        if (Vector3.Distance(gameObject.transform.position, billiardBall.transform.position) < length + 0.03)
        {
            stickHitBallAudioSource.volume = Mathf.Abs(shotPower / 1.5f);
            stickHitBallAudioSource.Play();
            billiardBall.GetComponent<BilliardBall>().Speed = (gameObject.transform.position - previousBilliardStickPosition) / Time.deltaTime;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            shotStage = ShotStage.NOSHOT;
        }
    }

    public void ToggleViewfinder()
    {
        if (Input.GetButtonDown("Toggle Viewfinder"))
        {
            if (viewfinder.activeInHierarchy)
            {
                viewfinder.SetActive(false);
                isViewfinderEnabled = false;
            }
            else
            {
                viewfinder.SetActive(true);
                isViewfinderEnabled = true;
            }
        }
    }
    
	void Start ()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        shotPower = 0.01f;
        shotStage = ShotStage.NOSHOT;
        length = 1.48f;
        PrepareShot(GameObject.Find("White Ball"));
	}

	void Update ()
    {
        switch (shotStage)
        {
            case ShotStage.PREPARINGSHOT:
                PrepareShot(GameObject.Find("White Ball"));
                break;
            case ShotStage.AIMING:
                Aim(GameObject.Find("White Ball"));
                ToggleViewfinder();
                break;
            case ShotStage.POWERADJUSTING:
                AdjustPower();
                ToggleViewfinder();
                break;
            case ShotStage.SHOOTING:
                Shot(GameObject.Find("White Ball"));
                break;
            case ShotStage.NOSHOT:
                int numberOfMovingBalls = 0;
                for (int i = 0; i < 16; ++i)
                {
                    if (GameObject.Find("Billiard Balls").GetComponent<BilliardBalls>()[i].GetComponent<BilliardBall>().Speed != new Vector3(0, 0, 0))
                    {
                        ++numberOfMovingBalls;
                    }
                }
                if (numberOfMovingBalls == 0)
                {
                    if (players.isCheckingIfWhiteBallWillBePocketedWithEightBall)
                    {
                        FindObjectOfType<EndGameGUIController>().ShowEndGameGUI(players.wasWhiteBallPocketedInActualTurn ? (players.ActivePlayerIndex == 0 ? 1 : 0) : players.ActivePlayerIndex);
                    }
                    shotStage = ShotStage.PREPARINGSHOT;
                    if (players.WasFoul)
                    {
                        string[] messages = new string[2];
                        players.ChangePlayer();
                        players.SetWhiteBilliardBall(GameObject.Find("White Ball"));
                        messages[0] = "Foul";
                        messages[1] = players.GetActivePlayerName() + "'s turn";
                        FindObjectOfType<MessagesController>().ShowMessagesSequence(messages);
                    }
                    else if (!players.checkBalls())
                    {
                        players.ChangePlayer();
                        FindObjectOfType<MessagesController>().ShowMessage(players.GetActivePlayerName() + "'s turn");
                        players.StickUsed = false;
                        players.setBalls();  
                    }
                    else
                    {
                        players.setBalls();
                    }
                    if (!players.WhiteBilliardBall)
                        players.StickUsed = true;
                    else
                        players.WhiteBilliardBall = false;
                }
                break;
        }
	}
}