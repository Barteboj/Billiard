using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

    [SerializeField]
	private GameObject followingCamera;

    public GameObject FollowingCamera
    {
        get
        {
            return followingCamera;
        }
        set
        {
            followingCamera = value;
        }
    }

	// Use this for initialization
	void Start () {
		//followingCamera.transform.rotation = new Quaternion (45, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		followingCamera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.5f, gameObject.transform.position.z);
	}
}
