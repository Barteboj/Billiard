using UnityEngine;
using System.Collections;

public class FireworkParticle : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    private bool isFlying = false;
    private float lifeTimePast = 0f;

    void Update()
    {
        if (isFlying)
        {
            Fly();
            lifeTimePast += Time.deltaTime;
            if (lifeTimePast > lifeTime)
            {
                Destroy(gameObject);
            }
            gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().color.r, gameObject.GetComponent<SpriteRenderer>().color.g, gameObject.GetComponent<SpriteRenderer>().color.b, Mathf.Lerp(1f, 0f, lifeTimePast / lifeTime));
        }
    }

    public void Fly()
    {
        gameObject.transform.Translate(gameObject.transform.up * speed * Time.deltaTime, Space.World);
    }

    public void StartFlying()
    {
        isFlying = true;
    }
}