using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CameraState { TOPDOWN, FIRSTPERSON }

public class CameraController : MonoBehaviour
{

    public Transform billiardStickTransform;
    public Transform billiardTableTransform;
    public Transform whiteBallTransform;

    public float topDownHeight = 1000;
    public float firstPersonHeight = 30;
    public float firstPersonDistance = 3;
    public Text rotateCameraText;
    public Text moveCameraText;

    private CameraState cameraState;

    public void SetCameraState(CameraState cameraState)
    {
        switch (cameraState)
        {
            case CameraState.TOPDOWN:
                this.cameraState = CameraState.TOPDOWN;
                gameObject.transform.position = new Vector3(billiardTableTransform.position.x, billiardTableTransform.position.y + topDownHeight, billiardTableTransform.position.z);
                gameObject.transform.right = Vector3.forward;
                gameObject.transform.Rotate(0, 0, 90, Space.World);
                rotateCameraText.enabled = false;
                moveCameraText.enabled = true;
                break;
            case CameraState.FIRSTPERSON:
                this.cameraState = CameraState.FIRSTPERSON;
                gameObject.transform.rotation = billiardStickTransform.rotation;
                gameObject.transform.Rotate(new Vector3(firstPersonHeight, 0, 0));
                gameObject.transform.position = whiteBallTransform.position;
                gameObject.transform.Translate(-gameObject.transform.forward * firstPersonDistance, Space.World);
                rotateCameraText.enabled = true;
                moveCameraText.enabled = false;
                break;
        }
    }

    public void ToggleCamera()
    {
        if (Input.GetButtonDown("Toggle Camera"))
        {
            if (cameraState == CameraState.TOPDOWN)
            {
                SetCameraState(CameraState.FIRSTPERSON);
            }
            else
            {
                SetCameraState(CameraState.TOPDOWN);
            }
        }
    }

    public void ZoomCamera()
    {
        switch (cameraState)
        {
            case CameraState.TOPDOWN:
                topDownHeight -= Input.GetAxis("Camera Zoom") * 1.7f * Time.deltaTime;
                break;
            case CameraState.FIRSTPERSON:
                firstPersonDistance -= Input.GetAxis("Camera Zoom") * Time.deltaTime;
                break;
        }
    }

    public void UpdatePointOfView()
    {
        switch (cameraState)
        {
            case CameraState.TOPDOWN:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, billiardTableTransform.position.y + topDownHeight, gameObject.transform.position.z);
                break;
            case CameraState.FIRSTPERSON:
                gameObject.transform.rotation = billiardStickTransform.rotation;
                gameObject.transform.Rotate(new Vector3(firstPersonHeight, 0, 0));
                gameObject.transform.position = whiteBallTransform.position;
                gameObject.transform.Translate(-gameObject.transform.forward * firstPersonDistance, Space.World);
                break;
        }
    }

    public void RotateCamera()
    {
        firstPersonHeight += Input.GetAxis("Camera Rotation") * 7 * Time.deltaTime;
    }

    public void MoveCamera()
    {
        gameObject.transform.Translate(-Input.GetAxis("Vertical") * Time.deltaTime, 0, Input.GetAxis("Horizontal") * Time.deltaTime, Space.World);
    }

    // Use this for initialization
    void Start()
    {
        SetCameraState(CameraState.TOPDOWN);
    }

    // Update is called once per frame
    void Update()
    {
        switch (cameraState)
        {
            case CameraState.TOPDOWN:
                MoveCamera();
                break;
            case CameraState.FIRSTPERSON:
                RotateCamera();
                break;
        }
        ToggleCamera();
        ZoomCamera();
        UpdatePointOfView();
    }
}