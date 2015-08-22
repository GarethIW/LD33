using UnityEngine;
using System.Collections;

public class FireManager : ObjectPool
{
    public static FireManager Instance;

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
        //instance.transform.position = new Vector3(Random.Range(LeftRightBounds.x,LeftRightBounds.y), 0f, Player.transform.position.z + Random.Range(0f,InitialDistance));
        //instance.transform.localRotation = Quaternion.Euler(0f,Random.Range(0, 360),0f);

        //setupDistance -= Random.Range(Spacing * 0.5f, Spacing * 1.5f);
        instance.SetActive(false);

        base.InitInstance(instance);
    }
}
