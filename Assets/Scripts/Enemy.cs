using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{

    protected GameObject player;

    Ray shootRay;
    RaycastHit shootHit;
    public float MoveToRange = 20f;
    public float FireAtRange = 10f;
    protected AudioSource attackSound;
    public float speed = 5f;

    float coolDownTimer = 0f;
    float firerate = 0.15f;
    Rigidbody enemyRigidBody;

    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackSound = GetComponent<AudioSource>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }



    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    protected virtual void Update()
    {

        coolDownTimer += Time.deltaTime;

        if (checkRange(FireAtRange))
        {
            if (coolDownTimer >= firerate)
            {
                coolDownTimer = 0f;
                Fire();
            }
        }
        else if (checkRange(MoveToRange))
        {
            transform.position += getMoveTowardsVector(transform.position, player.transform.position);
        }
        else
        {
            transform.position += getMoveTowardsVector(transform.position, getExploringPoint());

        }

    }

    protected abstract void Fire();


    private bool checkRange(float range)
    {
        bool result = false;
        shootRay.origin = transform.position;
        shootRay.direction = transform.right;

        if (Physics.Raycast(shootRay, out shootHit, range))
        {

            if (shootHit.transform.tag == "Player")
            {

                result = true;

            }


        }
        return result;

    }

    Vector3 getMoveTowardsVector(Vector3 location, Vector3 target)
    {
        Vector3 targetLocation = (target - location).normalized * speed * Time.deltaTime;
        return targetLocation;
    }

    Vector3 getExploringPoint()
    {
        Vector3 position = RandomPoint(transform.position, 5);




        return position;
    }

    Vector3 RandomPoint(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        return pos;
    }




}
