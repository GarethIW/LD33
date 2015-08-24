using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class Projectile : MonoBehaviour
{
    
    private ParticleSystem parts;
    
    // Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //time -= Time.deltaTime;
     //   if(time<=0f) gameObject.SetActive(false);
	}

    public void Init(Vector3 dir, float velocity)
    {
        //transform.localScale = Vector3.one*scale;

        //parts = GetComponent<ParticleSystem>();
        //parts.Play();

        //time = 8f;
        gameObject.SetActive(true);

        GetComponent<Rigidbody>().AddForce(dir * velocity, ForceMode.Force);

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInParent<Kaiju>() != null && collision.collider.name != "hand1")
        {
            collision.collider.GetComponentInParent<Kaiju>().HitByProjectile(transform.position);

            var ex = ExplosionManager.Instance.GetOne("Explosion");
            if (ex != null)
            {
                ex.transform.position = transform.position + new Vector3(0f, 0f, -0.1f);
                ex.GetComponent<Explosion>().Init(0.5f);
            }

            gameObject.SetActive(false);
        }

        if (collision.collider.name == "Floor")
        {
            var ex = ExplosionManager.Instance.GetOne("Explosion");
            if (ex != null)
            {
                ex.transform.position = transform.position + new Vector3(0f, 0f, -0.1f);
                ex.GetComponent<Explosion>().Init(0.5f);
            }

            gameObject.SetActive(false);
        }
    }

}
