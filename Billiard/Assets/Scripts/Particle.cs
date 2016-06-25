using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    public float lifeTime;

    public float speed;

    private float bornTime;

    private Material material;
    
	void Start ()
    {
        bornTime = Time.time;
        material = gameObject.GetComponent<MeshRenderer>().material;
        Destroy(gameObject, lifeTime);
	}
	
	void Update ()
    {
        gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
        material.color = new Color(material.color.r, material.color.g, material.color.b, 1 - ((Time.time - bornTime) / lifeTime));
	}
}