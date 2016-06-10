using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    public float lifeTime;
    public float speed;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Translate(-speed * Time.deltaTime, 0, 0, Space.World);
	}
}
