using UnityEngine;
using System.Collections;

public class Firework : MonoBehaviour
{
    public FireworkParticle fireworkParticle;
    public AudioSource fireworkTakeOffAudioSource;
    public AudioSource fireworkBlastAudioSource;
    public float speed;
    public float timeToExplode;
    public int amountOfParticlesFromExplosion;
    public float minParticleSpeed;
    public float maxParticleSpeed;
    public float minParticleLifetime;
    public float maxParticleLifetime;
    public Color[] particlesColors;
    private float timePast = 0f;
    private bool isFlying;

    void Update()
    {
        if (isFlying)
        {
            Fly();
            timePast += Time.deltaTime;
            if (timePast > timeToExplode)
            {
                Explode();
            }
        }
    }

    public void StartFlying()
    {
        isFlying = true;
        fireworkTakeOffAudioSource.Play();
    }

    public void Fly()
    {
        gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
    }

    public void Explode()
    {
        fireworkTakeOffAudioSource.Stop();
        fireworkBlastAudioSource.Play();
        for (int i = 0; i < amountOfParticlesFromExplosion; ++i)
        {
            GameObject instantiatedFirework = (GameObject)Instantiate(fireworkParticle.gameObject, gameObject.transform.position, gameObject.transform.rotation);
            instantiatedFirework.transform.Rotate(instantiatedFirework.transform.forward, Random.Range(0f, 360f), Space.World);
            instantiatedFirework.GetComponent<SpriteRenderer>().color = particlesColors[Random.Range(0, particlesColors.Length)];
            instantiatedFirework.GetComponent<FireworkParticle>().lifeTime = Random.Range(minParticleLifetime, maxParticleLifetime);
            instantiatedFirework.GetComponent<FireworkParticle>().speed = Random.Range(minParticleSpeed, maxParticleSpeed);
            instantiatedFirework.GetComponent<FireworkParticle>().StartFlying();
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, fireworkBlastAudioSource.clip.length);
        isFlying = false;
    }
}