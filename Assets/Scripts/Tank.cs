using UnityEngine;
using System.Collections;

public class Tank : Enemy
{

    private Transform body;
   // private Transform barrel;

    public override void Awake()
    {
        body = transform.Find("Body");
     //   barrel = transform.Find("Barrel");
        base.Awake();
    }

    protected override void Update()
    {
        coolDownTimer += Time.deltaTime;
        movementCooldowntimer += Time.deltaTime;

        if (isOnFire)
        {
            Health -= FlameDamage;
        }

        if (transform.gameObject.activeSelf && Health <= 0f)
        {
            GameManager.Instance.score += ScorePoints;
            GameManager.Instance.DamageCost += DamageValue;

            var ex = ExplosionManager.Instance.GetOne("Explosion");
            if (ex != null)
            {
                ex.transform.position = transform.position + new Vector3(0f,0.25f,-0.1f);
                ex.GetComponent<Explosion>().Init(1.5f);
            }

            gameObject.SetActive(false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < FireAtRange)
        {
            isFiring = true;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < MoveToRange)
        {
            isFiring = false;

            currentMovementTarget = RandomPoint(player.transform.position +
                                    ((transform.position - player.transform.position).normalized * (FireAtRange - 1f)), 1f);
            currentMovementTarget.z = transform.position.z;
            //transform.position += getMoveTowardsVector(transform.position, player.transform.position);
        }
        else
        {
            isFiring = false;
            currentMovementTarget = getExploringPoint();
            currentMovementTarget.z = transform.position.z;
        }


        if (isFiring)
        {
            currentMovementTarget = transform.position;

            if (coolDownTimer >= FireRate && Random.Range(0, 10) == 0)
            {
                coolDownTimer = 0f;
                Fire();
            }

            //spriteRender.sprite = frames[2];

            if (player.transform.position.x > transform.position.x) faceDir = -1;
            else faceDir = 1;
        }
        else
        {
            if (transform.position.x < currentMovementTarget.x)
                faceDir = -1;
            else if (transform.position.x > currentMovementTarget.x)
                faceDir = 1;
        }

        body.transform.localScale = new Vector3(2f * faceDir, 1f, 1f);
    //    barrel.transform.localScale = new Vector3(1f * faceDir, 0.1f, 1f);

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

    protected override void Fire()
    {
        if (
            !(player.transform.position.z > transform.position.z - 0.5f &&
              player.transform.position.z < transform.position.z + 0.5f)) return;

        var p = ProjectileManager.Instance.GetOne("Projectile");
        if (p != null)
        {
            p.transform.position = transform.position + new Vector3(-faceDir* 0.5f, 0.5f, 0.01f);
            //Vector3 dir = (player.transform.position - transform.position).normalized;
            Vector3 dir = new Vector3(-faceDir, 0.4f,0f).normalized;
            p.GetComponent<Projectile>().Init(dir, 40f);
        }
    }


    protected override void SetUpAudio()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {

            if (source.clip.name.Equals("chicken"))
            {
                moveAudio = source;
            }
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInParent<Kaiju>() != null && collision.collider.name!="hand1")
        {
            player.GetComponent<Kaiju>().Stomp();
        }

        if (collision.collider.name == "Floor")
        {
            isOnFloor = true;
            transform.localRotation = Quaternion.identity;
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }
}
