using UnityEngine;
using System.Collections;

public class FireworksEmitter : MonoBehaviour
{
    public Firework firework;

    public float areaWidth;
    public float minDelay;
    public float maxDelay;
    public float minAngle;
    public float maxAngle;
    public float minSpeed;
    public float maxSpeed;
    public float minTimeToExplode;
    public float maxTimeToExplode;
    public int minParticleAmount;
    public int maxParticleAmount;
    public float minParticleSpeed;
    public float maxParticleSpeed;
    public float minParticleLifetime;
    public float maxParticleLifetime;
    public FireworkColor[] fireWorksColors;

    void Start()
    {
        EmitFireworks();
    }

    public IEnumerator EmitFireworksRoutine()
    {
        GameObject instantiatedFirework = (GameObject)Instantiate(firework.gameObject, gameObject.transform.position + gameObject.transform.right * Random.Range(-areaWidth / 2f, areaWidth / 2f), gameObject.transform.rotation);
        instantiatedFirework.transform.Rotate(instantiatedFirework.transform.forward, Random.Range(minAngle, maxAngle), Space.World);
        instantiatedFirework.GetComponent<Firework>().speed = Random.Range(minSpeed, maxSpeed);
        instantiatedFirework.GetComponent<Firework>().timeToExplode = Random.Range(minTimeToExplode, maxTimeToExplode);
        instantiatedFirework.GetComponent<Firework>().amountOfParticlesFromExplosion = Random.Range(minParticleAmount, maxParticleAmount);
        instantiatedFirework.GetComponent<Firework>().minParticleSpeed = minParticleSpeed;
        instantiatedFirework.GetComponent<Firework>().maxParticleSpeed = maxParticleSpeed;
        instantiatedFirework.GetComponent<Firework>().minParticleLifetime = minParticleLifetime;
        instantiatedFirework.GetComponent<Firework>().maxParticleLifetime = maxParticleLifetime;
        int randomColorIndex = Random.Range(0, fireWorksColors.Length);
        instantiatedFirework.GetComponent<SpriteRenderer>().color = fireWorksColors[randomColorIndex].mainColor;
        instantiatedFirework.GetComponent<Firework>().particlesColors = fireWorksColors[randomColorIndex].particlesColors;
        instantiatedFirework.GetComponent<Firework>().StartFlying();
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        StartCoroutine(EmitFireworksRoutine());
    }

    public void EmitFireworks()
    {
        StartCoroutine(EmitFireworksRoutine());
    }
}