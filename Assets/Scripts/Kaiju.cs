using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class Kaiju : MonoBehaviour
{
    public static Kaiju Instance;

    public bool CanClimb = false;

    public float Depth = 0f;

    public bool isClimbing = false;
    public bool isOnGround = false;
    public GameObject climbingSkyscraperSection;

    public Transform BreathTransform;

    SkeletonAnimation skeletonAnimation;
    public string idleAnimation = "empty";
    public string walkAnimation = "walk";
    public string runAnimation = "walk";
    public string hitAnimation = "hit";
    public string deathAnimation = "death";
    public float walkVelocity = 1;
    public float runVelocity = 3;
    public float climbVelocity = 1;
    public float climbGravity = 1;
    public float maxSpeed = 100f;
    public int hp = 10;
    string currentAnimation = "";
    bool hit = false;
    bool dead = false;
    private float faceDir = 1f;

    private Rigidbody rigidBody;
    private PlaygroundParticlesC breathParticles;

    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rigidBody = GetComponent<Rigidbody>();
        breathParticles = PlaygroundC.GetParticles(0);
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


        if (!isClimbing)
        {
            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            gameObject.layer = 9;
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

            if (absX > 0.7f)
            {
                SetAnimation(runAnimation, true);
                //GetComponent<Rigidbody>().velocity = new Vector3(runVelocity * Mathf.Sign(x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
            }
            else if (absX > 0)
            {
                SetAnimation(walkAnimation, true);
                // GetComponent<Rigidbody>().velocity = new Vector3(walkVelocity * Mathf.Sign(x), GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
            }


            if (absY > 0.7f)
            {
                SetAnimation(runAnimation, true);
                // GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, runVelocity * Mathf.Sign(y));
            }
            else if (absY > 0)
            {
                SetAnimation(walkAnimation, true);
                // GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, walkVelocity * Mathf.Sign(y));
            }

            rigidBody.velocity = new Vector3(walkVelocity * xy.x, rigidBody.velocity.y,
                walkVelocity * xy.z);

            if (absX <= 0 && absY <= 0)
            {
                SetAnimation(idleAnimation, true);
                rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
            }

            if (CanClimb && Input.GetButtonDown("Climb"))
            {
                isClimbing = true;
                gameObject.layer = 11;
                if (climbingSkyscraperSection != null)
                {
                    float dir = transform.position.x > climbingSkyscraperSection.transform.parent.position.x ? -1f : 1f;
                    if (dir == -1f) skeletonAnimation.skeleton.flipX = true;
                    else skeletonAnimation.skeleton.flipX = false;
                    transform.position = new Vector3(climbingSkyscraperSection.transform.parent.position.x + (0.9f * -dir), transform.position.y+0.1f, climbingSkyscraperSection.transform.position.z);
                    rigidBody.AddForce(0f, climbVelocity, 0f, ForceMode.Acceleration);
                }
            }
        }
        else
        {
            rigidBody.useGravity = false;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            gameObject.layer = 11;
            maxSpeed = 2f;

            if (Input.GetButtonDown("Climb"))
            {
                if (climbingSkyscraperSection != null)
                {
                    float dir = transform.position.x > climbingSkyscraperSection.transform.parent.position.x ? -1f : 1f;
                    if (dir == -1f) skeletonAnimation.skeleton.flipX = true;
                    else skeletonAnimation.skeleton.flipX = false;
                    transform.position = new Vector3(climbingSkyscraperSection.transform.parent.position.x + (0.9f * -dir), transform.position.y, climbingSkyscraperSection.transform.position.z);
                    rigidBody.AddForce(0f, climbVelocity, 0f, ForceMode.Acceleration);

                }

            }
            else
            {
                rigidBody.AddForce(0f, climbGravity, 0f, ForceMode.Acceleration);
            }

            SetAnimation(idleAnimation, true);
        }

        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }

        if (Input.GetButton("Breathe"))
        {
            for(int i=0;i<50;i++)
                breathParticles.Emit(BreathTransform.position,new Vector3(Random.Range(4f, 12f) * faceDir, Random.Range(-1.9f, -2.1f), 0f));
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

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Skyscraper>() != null)
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
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Skyscraper>() != null)
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
