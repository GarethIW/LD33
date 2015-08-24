using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class Explosion : MonoBehaviour
{
    float time = 5f;

    private ParticleSystem parts;
    
    // Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    time -= Time.deltaTime;
        if(time<=0f) gameObject.SetActive(false);
	}

    public void Init(float scale)
    {
        transform.localScale = Vector3.one*scale;

        parts = GetComponent<ParticleSystem>();
        parts.Play();

        time = 8f;
        gameObject.SetActive(true);
    }
  
}
