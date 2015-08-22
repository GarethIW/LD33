using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class SkyscraperSection : MonoBehaviour
{
    public float Health = 10f;

    private PlaygroundEventC playgroundEvent;

    // Use this for initialization
    void Start()
    {
        playgroundEvent = PlaygroundC.GetEvent(0, PlaygroundC.GetParticles(0));
        playgroundEvent.particleEvent += OnEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health < 10f)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Health,Health,Health));    
        }

        if(Health<=0f)
            gameObject.SetActive(false);
    }

    void OnEvent(PlaygroundEventParticle particle)
    {
        if (this == null) return;
        if (particle.collisionCollider.gameObject == this.gameObject)
        {
            Health -= 0.001f;
            if (Random.Range(0, 500) == 0)
            {
                var fire = FireManager.Instance.GetOne("Fire");
                if (fire != null)
                {
                    fire.transform.position = transform.position +
                                              new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), -0.01f);
                    fire.GetComponent<Fire>().Init();
                }
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInParent<Skyscraper>() != null)
        {
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Skyscraper>() != null)
        {
        }
    }
}
