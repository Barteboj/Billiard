using UnityEngine;
using System.Collections;

public class ParticleEmitter : MonoBehaviour {

    public float emitterWidth;
    public float speedMin, speedMax;
    public float lifeTime;
    public float generationFrequency;
    public float scaleMin, scaleMax;
    public float minAngle, maxAngle;
    public float minRColor, maxRColor;
    public float minGColor, maxGColor;
    public float minBColor, maxBColor;
    public GameObject particleSample;
    private bool areParticlesToBeDestroyed = false;
    private CameraController cameraController;
    private GameObject billiardStick;

    public void SignalDestroyingAllParticles()
    {
        areParticlesToBeDestroyed = true;
    }

    private void DestroyAllParticles()
    {
        Particle[] particles = FindObjectsOfType<Particle>();
        for (int i = 0; i < particles.Length; ++i)
        {
            Destroy(particles[i].gameObject);
        }
        /*foreach (Particle particle in FindObjectsOfType<Particle>())
        {
            Destroy(particle.gameObject);
        }*/
        areParticlesToBeDestroyed = false;
    }

    public void GenerateParticle()
    {
        if (areParticlesToBeDestroyed)
        {
            DestroyAllParticles();
        }
        float randomRange = Random.Range(scaleMin, scaleMax);
        particleSample.transform.localScale = new Vector3(randomRange, randomRange, randomRange);
        GameObject newSample = (GameObject)Instantiate(particleSample, gameObject.transform.position + gameObject.transform.right * Random.Range(-emitterWidth / 2, emitterWidth / 2), gameObject.transform.rotation);
        newSample.SetActive(true);
        newSample.GetComponent<Particle>().lifeTime = lifeTime;
        newSample.GetComponent<Particle>().speed = Random.Range(speedMin, speedMax);
        newSample.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(minRColor, maxRColor), Random.Range(minGColor, maxGColor), Random.Range(minBColor, maxBColor));
        newSample.transform.Rotate(newSample.transform.forward, Random.Range(minAngle, maxAngle), Space.World);
        newSample.GetComponent<MeshRenderer>().enabled = true;
        newSample.GetComponent<Particle>().enabled = true;
    }

    public IEnumerator GenerateParticles()
    {
        GenerateParticle();
        yield return new WaitForSeconds(1 / generationFrequency);
        StartCoroutine(GenerateParticles());
    }
    
	void OnEnable ()
    {
        billiardStick = FindObjectOfType<BilliardStick>().gameObject;
        cameraController = FindObjectOfType<CameraController>();
        StartCoroutine(GenerateParticles());
	}
	
	void Update ()
    {

	}
}