using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    public Kaiju Kaiju;
    public Vector3 Offset;
    public Vector3 Target;
    public float Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
        //Target = new Vector3(Kaiju.transform.position.x, Kaiju.transform.position.y, Kaiju.Depth*2f) + Offset;
        Target = Kaiju.transform.position+ Offset;
        transform.position = Vector3.Lerp(transform.position, Target, Time.deltaTime*Speed);
	}

    
}
