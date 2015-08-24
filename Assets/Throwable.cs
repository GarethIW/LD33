using UnityEngine;
using System.Collections;

public class Throwable : MonoBehaviour
{
    public Vector3 HandleOffset;
    public Vector3 StartScale;
    public float ThrowForce;
    public bool isBeingThrown;

    // Use this for initialization
    void Start()
    {
        StartScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Throw(Vector3 dir)
    {
        transform.SetParent(null);
        transform.localScale = StartScale;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().WakeUp();
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;

        if (GetComponent<Tank>() != null) GetComponent<Tank>().enabled = true;

        GetComponent<Rigidbody>().AddForce(dir * ThrowForce, ForceMode.Force);
        isBeingThrown = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "hand1" && !isBeingThrown && other.GetComponentInParent<Kaiju>().isPickingUp && other.GetComponentInParent<Kaiju>().holdingObject==null)
        {
            
            Debug.Log("picked up " + gameObject.name);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().Sleep();
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            if (GetComponent<Tank>() != null) GetComponent<Tank>().enabled = false;

            other.GetComponentInParent<Kaiju>().Pickup(gameObject, HandleOffset);

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Floor" && isBeingThrown)
        {
            isBeingThrown = false;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

            foreach (Collider hit in colliders)
            {
                if (hit.GetComponent<Rigidbody>() && !hit.GetComponent<SkyscraperSection>())
                    hit.GetComponent<Rigidbody>().AddExplosionForce(200f, transform.position, 3f, 2f);

                if (hit.GetComponent<Enemy>() != null)
                    hit.GetComponent<Enemy>().KnockedBack();
            }
        }
    }
}
