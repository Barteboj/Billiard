using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CameraState
{
    TOPDOWN,
    FIRSTPERSON
}

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
    public Vector2 topDownPosition;

    public CameraState cameraState;

    public void SetCameraState(CameraState cameraState)
    {
        foreach (ParticleEmitter particleEmitter in FindObjectsOfType<ParticleEmitter>())
        {
            particleEmitter.SignalDestroyingAllParticles();
        }
        switch (cameraState)
        {
            case CameraState.TOPDOWN:
                this.cameraState = CameraState.TOPDOWN;
                gameObject.transform.position = new Vector3(topDownPosition.x, billiardTableTransform.position.y + topDownHeight, topDownPosition.y);
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
                topDownHeight = Mathf.Clamp(topDownHeight, 0.12f, 2.3f);
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
                gameObject.transform.position = new Vector3(topDownPosition.x, billiardTableTransform.position.y + topDownHeight, topDownPosition.y);
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
        firstPersonHeight = Mathf.Clamp(firstPersonHeight, 2, 30);
    }

    public void MoveCamera()
    {
        topDownPosition += new Vector2(-Input.GetAxis("Vertical") * Time.deltaTime, Input.GetAxis("Horizontal") * Time.deltaTime);
        topDownPosition.x = Mathf.Clamp(topDownPosition.x, -1.1f, 1.7f);
        topDownPosition.y = Mathf.Clamp(topDownPosition.y, -3.5f, 3f);
    }

    void Start()
    {
        topDownPosition = new Vector2(billiardTableTransform.position.x, billiardTableTransform.position.z);
        SetCameraState(CameraState.TOPDOWN);
    }

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