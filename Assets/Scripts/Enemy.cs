using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ParticlePlayground;
using UnityEngine.Rendering;

public abstract class Enemy : MonoBehaviour
{

    protected GameObject player;

    public Sprite[] frames;

    public float MoveToRange = 20f;
    public float FireAtRange = 10f;

    public float Speed = 5f;
    public float FlameDamage = 0.1f;
    public float KnockbackDamage = 0.1f;
    public float Health = 10f;
    public float BaseHealth = 10f;
    public int ScorePoints = 10;
    public float FireRate = 0.15f;
    public float MovementCoolDown = 1f;
    public float coolDownTimer = 0f;
    public float movementCooldowntimer = 0f;
    public int DamageValue = 5;

    public bool isOnFire;
    public bool isFleeing;
    public bool isFiring;

    private List<Fire> fires = new List<Fire>();

    protected AudioSource attackSound;
    public Vector3 currentMovementTarget;


    private Ray shootRay;
    private RaycastHit shootHit;

    private Rigidbody enemyRigidBody;
    private PlaygroundEventC playgroundEvent;

    private CityManager theCity;

    protected int male = 0;
    protected float animTime;
    protected int animFrame;

    protected int faceDir;

    // Use this for initialization
    void Start()
    {
        playgroundEvent = PlaygroundC.GetEvent(0, PlaygroundC.GetParticles(0));
        playgroundEvent.particleEvent += OnEvent;


    }

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
            //Debug.Log(" OnEvent() Health:" + Health);
            if (Random.Range(0, 10) == 0)
            {
                var fire = FireManager.Instance.GetOne("SmallFire");
                if (fire != null)
                {
                    fire.GetComponent<Fire>().Init(transform, new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0f, 0.4f), -0.01f));
                    fires.Add(fire.GetComponent<Fire>());
                    //fire.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.25f, 0.25f), -0.01f);
                    //fire.GetComponent<Fire>().Init();
                    //fire.transform.SetParent(transform);
                    Escape();
                }
            }
        }
    }


    protected void Escape()
    {

        float directionToMove = 5f;
        if (player.transform.position.x > transform.position.x)
        {
            directionToMove = -5f;
        }

        float newX = transform.position.x + directionToMove;

        //if (newX < theCity.getCityBoundryWidth() && newX > 0)
        //{
        currentMovementTarget = transform.position;
        currentMovementTarget.x = newX;
        //    //transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);
        //}
    }




    // Update is called once per frame
    protected virtual void Update()
    {

        coolDownTimer += Time.deltaTime;
        movementCooldowntimer += Time.deltaTime;

        if (isOnFire)
        {
            Health -= FlameDamage;

            //if (Random.Range(0, 200) == 0)
            //{
            //    getExploringPoint();
            //}

            //Debug.Log("Update() Health" + Health);
        }

        if (transform.gameObject.activeSelf && Health <= 0f)
        {
            GameManager.Instance.score += ScorePoints;
            GameManager.Instance.DamageCost += DamageValue;

            //Debug.Log("Update() Score " + GameManager.Instance.score);
            gameObject.SetActive(false);
        }

        //if (checkRange(FireAtRange) && !isFleeing && !isOnFire)
        //{
        //    if (coolDownTimer >= FireRate)
        //    {
        //        coolDownTimer = 0f;
        //        Fire();
        //    }
        //}
        //else if (checkRange(MoveToRange))
        //{
        //    transform.position += getMoveTowardsVector(transform.position, player.transform.position);
        //}
        //else
        //{

        //    if (movementCooldowntimer >= MovementCoolDown)
        //    {
        //        currentMovementTarget = getExploringPoint();

        //        movementCooldowntimer = 0f;
        //    }

        //    transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);

        //}

        transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);







        int fireCount = 0;
        for (int f = fires.Count - 1; f >= 0; f--)
        {
            if (fires[f].Health <= 0 || !fires[f].gameObject.activeSelf)
            {
                fires.RemoveAt(f);
            }
            else fireCount++;
        }

        isOnFire = fireCount > 0;


    }

    protected abstract void Fire();


    protected bool checkRange(float range)
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

    protected Vector3 getMoveTowardsVector(Vector3 location, Vector3 target)
    {
        Vector3 targetLocation = (target - location).normalized * Speed * Time.deltaTime;
        targetLocation.y = 0f;




        return targetLocation;
    }

    protected Vector3 getExploringPoint()
    {
        Vector3 position = RandomPoint(transform.position, 1f);




        return position;
    }

    protected Vector3 RandomPoint(Vector3 center, float radius)
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

    public virtual void Init()
    {
        Health = BaseHealth;
        male = Random.Range(0, 2);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInParent<Kaiju>()!=null)
        {
            //Debug.Log("Squished!");
            Health = 0f;
        }
    }

    public void KnockedBack()
    {
        Health -= KnockbackDamage;
        if (Random.Range(0, 5) == 0)
        {
            var fire = FireManager.Instance.GetOne("SmallFire");
            if (fire != null)
            {
                fire.GetComponent<Fire>().Init(transform, new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(0f, 0.4f), -0.01f));
                fires.Add(fire.GetComponent<Fire>());
                Escape();
            }
        }
    }
}
