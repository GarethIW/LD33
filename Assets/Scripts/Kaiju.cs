using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using ParticlePlayground;

public class Kaiju : MonoBehaviour
{
    public static Kaiju Instance;

    public bool CanClimb = false;

    public float Depth = 0f;

    public bool Dead;

    public bool isClimbing = false;
    public bool isOnGround = false;
    public bool isJumping = false;
    public bool isPickingUp = false;
    public bool isThrowing = false;

    public GameObject holdingObject;

    public GameObject climbingSkyscraperSection;

    public Transform BreathTransform;

    public Color Tint;

    private float roarTime = 6;

    SkeletonAnimation skeletonAnimation;
    public string idleAnimation = "empty";
    public string walkAnimation = "walk";
    public string runAnimation = "walk";
    public string hitAnimation = "hit";
    public string deathAnimation = "death";
    public string jumpAnimation = "jump";
    public string fallAnimation = "Fall";
    public string pickupAnimation = "Pickup";
    public string throwAnimation = "Throw";
    public float walkVelocity = 1;
    public float runVelocity = 3;
    public float climbVelocity = 1;
    public float climbGravity = 1;
    public float jumpVelocity = 10f;
    public float maxSpeed = 100f;
    public int Health = 10;
    public int BaseHealth = 10;
    string currentAnimation = "";
    bool hit = false;
    bool dead = false;
    private float faceDir = 1f;

    private Color prevTint;

    private Rigidbody rigidBody;
    private PlaygroundParticlesC breathParticles;
    private Transform collider;
    private ParticleSystem stompParticles;
    private Transform handTransform;

    Dictionary<string,AudioSource> audioSources = new Dictionary<string, AudioSource>();

    private float pickupThrowTime;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rigidBody = GetComponent<Rigidbody>();
        breathParticles = PlaygroundC.GetParticles(0);

        //SkeletonUtility skelut = GetComponent<SkeletonUtility>();
        skeletonAnimation.skeleton.slots.ForEach(sl => { sl.SetColor(Tint); });
        prevTint = Tint;

        skeletonAnimation.state.Event += State_Event;

        collider = transform.FindChild("Collider");

        stompParticles = GameObject.Find("Explosion_circle").GetComponent<ParticleSystem>();

        handTransform = transform.Find("SkeletonUtility-Root/root/Hip/body/upperarm1/forearm1/hand1");

        Health = BaseHealth;

        foreach (AudioSource source in GetComponents<AudioSource>())
        {
            audioSources.Add(source.clip.name, source);
        }

        audioSources["Fire"].Play();
        audioSources["Fire"].Pause();
    }

    private void State_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        if (e.Data.Name == "Footstep")
        {
            audioSources["Kaiju_FootStep"].pitch = Random.Range(0.9f, 1.1f);
            audioSources["Kaiju_FootStep"].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Depth = ((int) (transform.position.z/4f))*2f;

        float x = Input.GetAxis("Horizontal");
        float absX = Mathf.Abs(x);

        float y = Input.GetAxis("Vertical");
        float absY = Mathf.Abs(y);


        Vector3 xy = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

        if (Health <= 0)
        {
            Dead = true;
            SetAnimation("Dead", false);

            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            gameObject.layer = 9;
            collider.gameObject.layer = 9;
        }

        if (!Dead)
        {
            if (Health <= 0)
            {
                
                audioSources["Kaiju_Roar"].pitch = 0.7f;
                audioSources["Kaiju_Roar"].Play();
            }

            if (!isClimbing && !isPickingUp && !isThrowing)
            {
                rigidBody.useGravity = true;
                rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                gameObject.layer = 9;
                collider.gameObject.layer = 9;
                maxSpeed = 5f;

                if (x > 0)
                {
                    skeletonAnimation.skeleton.FlipX = false;
                    faceDir = 1f;
                }
                else if (x < 0)
                {
                    skeletonAnimation.skeleton.FlipX = true;
                    faceDir = -1f;
                }

                if (absX > 0.7f && !isJumping)
                {
                    SetAnimation(runAnimation, true);
                    //GetComponent<Rigidbody>().velocity = new Vector3(runVelocity * Mathf.Sign(x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
                }
                else if (absX > 0 && !isJumping)
                {
                    SetAnimation(walkAnimation, true);
                    // GetComponent<Rigidbody>().velocity = new Vector3(walkVelocity * Mathf.Sign(x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
                }


                if (absY > 0.7f && !isJumping)
                {
                    SetAnimation(runAnimation, true);
                    // GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, runVelocity * Mathf.Sign(y));
                }
                else if (absY > 0 && !isJumping)
                {
                    SetAnimation(walkAnimation, true);
                    // GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, walkVelocity * Mathf.Sign(y));
                }

                rigidBody.velocity = new Vector3(walkVelocity*xy.x, rigidBody.velocity.y,
                    walkVelocity*xy.z);

                if (absX <= 0 && absY <= 0 && !isJumping)
                {
                    SetAnimation(idleAnimation, true);
                    rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
                }

                if (CanClimb && Input.GetButton("Climb"))
                {
                    isClimbing = true;
                    gameObject.layer = 11;
                    collider.gameObject.layer = 11;
                    if (climbingSkyscraperSection != null)
                    {
                        float dir = transform.position.x > climbingSkyscraperSection.transform.parent.position.x
                            ? -1f
                            : 1f;
                        if (dir == -1f) skeletonAnimation.skeleton.flipX = true;
                        else skeletonAnimation.skeleton.flipX = false;
                        transform.position =
                            new Vector3(climbingSkyscraperSection.transform.parent.position.x + (0.9f*-dir),
                                transform.position.y + 0.1f, climbingSkyscraperSection.transform.position.z - 0.1f);
                        rigidBody.AddForce(0f, climbVelocity, 0f, ForceMode.Acceleration);
                        climbingSkyscraperSection.GetComponent<SkyscraperSection>().ClimbedOn();
                    }
                }

                if (isOnGround && Input.GetButtonDown("Jump"))
                {
                    rigidBody.AddForce(0f, jumpVelocity, 0f, ForceMode.Force);
                    transform.position += new Vector3(0f, 0.1f, 0f);
                    isJumping = true;
                    isClimbing = false;
                }

                if (isOnGround && Input.GetButtonDown("Throw") && holdingObject == null)
                {
                    isPickingUp = true;
                    pickupThrowTime = 0.25f;
                    SetAnimation("Pickup", false);
                }

                if (isJumping)
                {
                    if (rigidBody.velocity.y > 0f)
                        SetAnimation(jumpAnimation, true);
                    if (rigidBody.velocity.y <= 0f)
                        SetAnimation(fallAnimation, true);
                }
            }
            else
            {
                isJumping = false;
                rigidBody.useGravity = false;
                rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                                        RigidbodyConstraints.FreezeRotation;
                gameObject.layer = 11;
                collider.gameObject.layer = 11;
                maxSpeed = 2f;

                if (CanClimb && Input.GetButton("Climb"))
                {
                    if (climbingSkyscraperSection != null)
                    {
                        float dir = transform.position.x > climbingSkyscraperSection.transform.parent.position.x
                            ? -1f
                            : 1f;
                        if (dir == -1f) skeletonAnimation.skeleton.flipX = true;
                        else skeletonAnimation.skeleton.flipX = false;


                        transform.position =
                            new Vector3(climbingSkyscraperSection.transform.parent.position.x + (0.9f*-dir),
                                transform.position.y, climbingSkyscraperSection.transform.position.z - 0.1f);
                        //rigidBody.AddForce(0f, climbVelocity, 0f, ForceMode.Acceleration);
                        transform.position += new Vector3(0f,climbVelocity,0f);
                        climbingSkyscraperSection.GetComponent<SkyscraperSection>().ClimbedOn();
                        SetAnimation("Climb", true);
                    }

                }
                else
                {
                    rigidBody.AddForce(0f, climbGravity + (-(transform.position.y*4f)), 0f, ForceMode.Acceleration);
                    if (!isPickingUp && !isThrowing) SetAnimation(idleAnimation, true);
                }

                //if (isClimbing && rigidBody.velocity.y > 0f) SetAnimation("Climb", true);
                
            }

            if (rigidBody.velocity.magnitude > maxSpeed)
            {
                rigidBody.velocity = rigidBody.velocity.normalized*maxSpeed;
            }

            if (Input.GetButtonDown("Throw") && holdingObject != null)
            {
                isThrowing = true;
                pickupThrowTime = 0.5f;
                SetAnimation("Throw", false);
            }

            if (isPickingUp || isThrowing)
            {
                pickupThrowTime -= Time.deltaTime;

                if (isThrowing && pickupThrowTime <= 0.1f && holdingObject != null)
                {
                    Throw();
                }

                if (pickupThrowTime <= 0f)
                {
                    isPickingUp = false;
                    isThrowing = false;
                }
            }

            if ((isOnGround || isClimbing) && Input.GetButtonDown("Jump"))
            {
                rigidBody.AddForce(0f, jumpVelocity, 0f, ForceMode.Force);
                transform.position += new Vector3(0f, 0.01f, 0f);
                isJumping = true;
                isClimbing = false;
            }

            if (Input.GetButton("Breathe") || Input.GetAxisRaw("Breathe")>0)
            {
                audioSources["Fire"].UnPause();
                for (int i = 0; i < 50; i++)
                    breathParticles.Emit(BreathTransform.position,
                        new Vector3(Random.Range(8f, 15f)*faceDir, Random.Range(-5.4f, -5.6f), 0f));
            }
            else audioSources["Fire"].Pause();


            roarTime -= Time.deltaTime;
            if (roarTime <= 0f)
            {
                roarTime = Random.Range(2f, 10f);
                audioSources["Kaiju_Roar"].pitch = Random.Range(0.9f, 1.1f);
                audioSources["Kaiju_Roar"].Play();
            }
        }

        if (prevTint != Tint)
        {
            skeletonAnimation.skeleton.slots.ForEach(sl => { sl.SetColor(Tint); });
            prevTint = Tint;
        }
    }

    void SetAnimation(string anim, bool loop)
    {
        if (currentAnimation != anim)
        {
            skeletonAnimation.state.SetAnimation(0, anim, loop);
            currentAnimation = anim;
        }
    }

    public void Pickup(GameObject other, Vector3 off)
    {
        if (!isPickingUp || holdingObject!=null) return;

        other.transform.SetParent(handTransform, true);
        other.transform.position = handTransform.position + off;
        holdingObject = other;
    }

    public void Throw()
    {
        if (holdingObject == null) return;
        holdingObject.GetComponent<Throwable>().Throw(new Vector3(faceDir,0f,0f));
        holdingObject = null;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<SkyscraperSection>() != null)
        {
            CanClimb = true;
            climbingSkyscraperSection = other.transform.parent.gameObject;
            //    IsInside = Room.IsInside;
        }
       
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.collider.name == "Floor")
        {
            isOnGround = true;
            isClimbing = false;
            isJumping = false;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.name == "Floor")
        {
            if (isJumping)
            {
                stompParticles.transform.position = transform.position + new Vector3(0,0.1f,0);
                stompParticles.Emit(250);


                Stomp();

                 



                //rigidBody.AddExplosionForce(100f, transform.position,5f);
            }
        }
    }

    public void Stomp()
    {
        audioSources["Kaiju_Stomp"].Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>() && !hit.GetComponent<SkyscraperSection>())
                hit.GetComponent<Rigidbody>().AddExplosionForce(200f, transform.position, 3f, 2f);

            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().KnockedBack();
        }
    }

    public void HitByProjectile(Vector3 pos)
    {
        Health -= 50;
        for (int i = 0; i < 50; i++)
            PlaygroundC.GetParticles(3).Emit(pos + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), -0.1f), Random.insideUnitSphere * 0.5f);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<SkyscraperSection>() != null)
        {
            CanClimb = false;
            climbingSkyscraperSection = null;
        }
       
    }

    public void OnCollisionExit(Collision other)
    {
        if (other.collider.name == "Floor")
        {
            isOnGround = false;
        }
    }

    
}
