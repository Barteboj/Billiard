using UnityEngine;
using System.Collections;

public class ParticleEffect : MonoBehaviour {

    public float emitterWidth;
    public float speedMin, speedMax;
    public float lifeTime;
    public float generationFrequency;
    public float scaleMin, scaleMax;
    public GameObject particleSample;

    public void GenerateParticle()
    {
        float randomRange = Random.Range(scaleMin, scaleMax);
        particleSample.transform.localScale = new Vector3(randomRange, randomRange, randomRange);
        GameObject newSample = (GameObject)Instantiate(particleSample, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Random.Range(gameObject.transform.position.z - (emitterWidth / 2), gameObject.transform.position.z + (emitterWidth / 2))), new Quaternion(0, 0, 0, 0));
        newSample.GetComponent<Particle>().lifeTime = lifeTime;
        newSample.GetComponent<Particle>().speed = Random.Range(speedMin, speedMax);
        newSample.GetComponent<MeshRenderer>().enabled = true;
        newSample.GetComponent<Particle>().enabled = true;
    }

    public IEnumerator GenerateParticles()
    {
        GenerateParticle();
        yield return new WaitForSeconds(1 / generationFrequency);
        StartCoroutine(GenerateParticles());
    }

	// Use this for initialization
	void OnEnable () {
        StartCoroutine(GenerateParticles());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}