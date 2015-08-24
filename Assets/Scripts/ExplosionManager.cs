using UnityEngine;
using System.Collections;

public class ExplosionManager : ObjectPool
{
    public static ExplosionManager Instance;

	// Use this for initialization
	public override void Start ()
	{
	    Instance = this;

	    base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}

    internal override void InitInstance(GameObject instance)
    {

        instance.SetActive(false);

        base.InitInstance(instance);
    }
}
