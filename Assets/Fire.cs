using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class Fire : MonoBehaviour
{
    public float Health = 1f;

    public Transform Target;
    public Vector3 Offset;

    public bool isSmall;

    private PlaygroundParticlesC fireParticles;

	// Use this for initialization
	void Start ()
	{
	    fireParticles = PlaygroundC.GetParticles(isSmall?2:1);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (Target != null)
	    {
	        transform.position = Target.position + Offset;
	        if (!Target.gameObject.activeSelf)
	        {
	            Target = null;
	            Health = 0f;
	        }
	    }

	    for (int i = 0; i < (isSmall ? 20 : 10); i++)
	        fireParticles.Emit(transform.position);

	    Health -= Random.Range(0f, 0.01f);
        if(Health<=0f) gameObject.SetActive(false);
	}

    public void Init()
    {
        Health = 1f;
        gameObject.SetActive(true);
    }
    public void Init(Transform target, Vector3 offset)
    {
        Target = target;
        Offset = offset;
        transform.position = Target.position + Offset;
        Init();
    }
}
