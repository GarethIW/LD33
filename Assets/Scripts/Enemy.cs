using UnityEngine;
using System.Collections;
using ParticlePlayground;

public abstract class Enemy : MonoBehaviour
{

    protected GameObject player;

    Ray shootRay;
    RaycastHit shootHit;
    public float MoveToRange = 20f;
    public float FireAtRange = 10f;
    protected AudioSource attackSound;
    public float speed = 5f;

    float movementCooldowntimer = 0f;
    Vector3 currentMovementTarget;
    float coolDownTimer = 0f;
    public float FireRate = 0.15f;
    public float MovementCoolDown = 1f;
    Rigidbody enemyRigidBody;
    private PlaygroundEventC playgroundEvent;
    public float Health = 10f;

    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackSound = GetComponent<AudioSource>();
        enemyRigidBody = GetComponent<Rigidbody>();
    }


    void OnEvent(PlaygroundEventParticle particle)
    {
        if (this == null) return;
        if (particle.collisionCollider.gameObject == this.gameObject)
        {
            Health -= 0.001f;
            if (Random.Range(0, 500) == 0)
            {
                var fire = FireManager.Instance.GetOne("Fire");
                if (fire != null)
                {
                    fire.transform.position = transform.position +
                                              new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), -0.01f);
                    fire.GetComponent<Fire>().Init();
                    fire.transform.SetParent(transform);
                }
            }
        }
    }





    // Use this for initialization
    void Start()
    {
        playgroundEvent = PlaygroundC.GetEvent(0, PlaygroundC.GetParticles(0));
        playgroundEvent.particleEvent += OnEvent;


    }

    // Update is called once per frame
    protected virtual void Update()
    {

        coolDownTimer += Time.deltaTime;
        movementCooldowntimer += Time.deltaTime;

       // if (Health < 10f)
       // {
      //      GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Health, Health, Health));
      //  }

        if (Health <= 0f)
        {
            gameObject.SetActive(false);
        }






        if (checkRange(FireAtRange))
        {
            if (coolDownTimer >= FireRate)
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

            if (movementCooldowntimer >= MovementCoolDown)
            {
                currentMovementTarget = getExploringPoint();
              
                movementCooldowntimer = 0f;
            }
            
            transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);

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
        targetLocation.y = 0f;

        
       
        
        return targetLocation;
    }

    Vector3 getExploringPoint()
    {
        Vector3 position = RandomPoint(transform.position, 0.2f);




        return position;
    }

    Vector3 RandomPoint(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = 0f;

        if (pos.z < 0f)
        {
            pos.z = 0f;
        }
        return pos;
    }




}
