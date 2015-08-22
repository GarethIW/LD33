using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class Fire : MonoBehaviour
{
    public float Health = 1f;

    private PlaygroundParticlesC fireParticles;

	// Use this for initialization
	void Start ()
	{
	    fireParticles = PlaygroundC.GetParticles(1);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    for (int i = 0; i < 10; i++)
	        fireParticles.Emit(transform.position);

	    Health -= Random.Range(0f, 0.01f);
        if(Health<=0f) gameObject.SetActive(false);
	}

    public void Init()
    {
        Health = 1f;
        gameObject.SetActive(true);
    }
}
