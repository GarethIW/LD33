using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour
{
    public Vector3 HandleOffset;
    public float ThrowForce;
    public bool isBeingThrown;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Throw(Vector3 dir)
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().WakeUp();
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().AddForce(dir * ThrowForce, ForceMode.Force);
        isBeingThrown = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "hand1" && !isBeingThrown)
        {
            Debug.Log("picked up " + gameObject.name);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().Sleep();
            GetComponent<BoxCollider>().enabled = false;
            other.GetComponentInParent<Kaiju>().Pickup(gameObject, HandleOffset);

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Floor")
            isBeingThrown = false;
    }
}
