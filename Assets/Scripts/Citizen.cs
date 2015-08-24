using UnityEngine;

//using System;

public class Citizen : Enemy
{
    private SpriteRenderer spriteRender;
        
   
    // Update is called once per frame
    protected override void Update()
    {

      //  coolDownTimer += Time.deltaTime;
        movementCooldowntimer += Time.deltaTime;

        //if (transform.gameObject.activeSelf && GetComponentInChildren<Fire>() != null)
        //{

        //    Health -= FlameDamage;
        //    Debug.Log("Update() Health" + Health);
        //}

        //if (transform.gameObject.activeSelf && Health <= 0f)
        //{
        //    GameManager.Instance.score += 5;

        //    Debug.Log("Update() Score " + GameManager.Instance.score);
        //    gameObject.SetActive(false);
        //}


        if (!isOnFire && !isFleeing && movementCooldowntimer >= MovementCoolDown)
        {
            currentMovementTarget = getExploringPoint();

            movementCooldowntimer = 0f;
        }

        if (!isOnFire && Vector3.Distance(player.transform.position, transform.position) < 2f)
        {
            isFleeing = true;
        }

        if (isOnFire)
        {
            if (Random.Range(0, 100) == 0)
            {
                getExploringPoint();
            }
        }

        if (isFleeing)
        {
            Escape();

            if (Vector3.Distance(player.transform.position, transform.position) > 5f)
                isFleeing = false;
        }


        // transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);


        animTime += Time.deltaTime;
        if (animTime >= 0.1f)
        {
            animTime = 0f;
            animFrame = 1 - animFrame;

            spriteRender.sprite = frames[(2 * male) + animFrame];
        }

        if (transform.position.x < currentMovementTarget.x)
            faceDir = -1;
        else if (transform.position.x > currentMovementTarget.x)
            faceDir = 1;

        spriteRender.transform.localScale = new Vector3(1f * faceDir, 1f, 1f);

        base.Update();

    }

    protected override void Fire()
    {

    }

    public override void Init()
    {
        spriteRender = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();

        spriteRender.color = new Color(Random.Range(0.2f, 1f), Random.Range(0.2f, 1f), Random.Range(0.2f, 1f));

        base.Init();
    }

    protected override void SetUpAudio()

    {

        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {

            if (source.clip.name.Equals("Human Footstep"))
            {
                moveAudio = source;
            }
            else if (source.clip.name.Equals("Human Grunt"))
            {
                painAudio = source;
            }

        }
    }
}
