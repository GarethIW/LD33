using UnityEngine;
using System.Collections;
using ParticlePlayground;

public abstract class Enemy : MonoBehaviour
{

    protected GameObject player;


    public float MoveToRange = 20f;
    public float FireAtRange = 10f;

    public float Speed = 5f;
    public float FlameDamage = 0.1f;
    public float Health = 10f;
    public int ScorePoints = 10;
    public float FireRate = 0.15f;
    public float MovementCoolDown = 1f;



    protected AudioSource attackSound;
    Ray shootRay;
    RaycastHit shootHit;

    float movementCooldowntimer = 0f;
    private Vector3 currentMovementTarget;
    private float coolDownTimer = 0f;
    private Rigidbody enemyRigidBody;
    private PlaygroundEventC playgroundEvent;

    private CityManager theCity;



    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackSound = GetComponent<AudioSource>();
        enemyRigidBody = GetComponent<Rigidbody>();
        theCity = GameObject.FindGameObjectWithTag("City").GetComponent<CityManager>();
    }


    void OnEvent(PlaygroundEventParticle particle)
    {
        if (this == null) return;
        if (particle.collisionCollider.gameObject == this.gameObject)
        {
            Health -= FlameDamage;
            Debug.Log(" OnEvent() Health:" + Health);
            if (Random.Range(0, 500) == 0)
            {
                var fire = FireManager.Instance.GetOne("Fire");
                if (fire != null)
                {
                    fire.transform.position = transform.position +
                                              new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), -0.01f);
                    fire.GetComponent<Fire>().Init();
                    fire.transform.SetParent(transform);
                    escape();
                }
            }
        }
    }


    void escape()
    {

        float directionToMove = 5f;
        if (player.transform.position.x > transform.position.x)
        {
            directionToMove = -5f;
        }

        float newX = transform.position.x + directionToMove;

        if (newX < theCity.getCityBoundryWidth() && newX > 0)
        {
            currentMovementTarget = transform.position;
            currentMovementTarget.x = newX;
            transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);
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

        if (transform.gameObject.activeSelf&&GetComponentInChildren<Fire>() != null)
        {

            Health -= FlameDamage;
            Debug.Log("Update() Health" + Health);
        }

        if (transform.gameObject.activeSelf&&Health <= 0f)
        {
            GameManager.Instance.score += 5;

            Debug.Log("Update() Score " + GameManager.Instance.score);
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
        Vector3 targetLocation = (target - location).normalized * Speed * Time.deltaTime;
        targetLocation.y = 0f;




        return targetLocation;
    }

    Vector3 getExploringPoint()
    {
        Vector3 position = RandomPoint(transform.position, 1f);




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
