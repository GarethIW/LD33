﻿using UnityEngine;
using System.Collections;
using ParticlePlayground;

public class Trooper : Enemy
{
    private SpriteRenderer spriteRender;

    LineRenderer gunLine;
    public int DamagePerShot = 1;
   

    public override void Awake()
    {
        base.Awake();

        gunLine = GetComponent<LineRenderer>();
    }

    protected override void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < FireAtRange && !isFleeing && !isOnFire)
        {
            isFiring = true;


        }
        else if (Vector3.Distance(transform.position, player.transform.position) < MoveToRange)
        {
            isFiring = false;

            currentMovementTarget = RandomPoint(player.transform.position +
                                    ((transform.position - player.transform.position).normalized * (FireAtRange - 1f)), 1f);
            //transform.position += getMoveTowardsVector(transform.position, player.transform.position);
        }
        else
        {
            isFiring = false;
            currentMovementTarget = getExploringPoint();
        }


        gunLine.enabled = false;


        if (isFiring)
        {
            currentMovementTarget = transform.position;


            if (coolDownTimer >= FireRate && Random.Range(0, 25) == 0)
            {
                coolDownTimer = 0f;
                Fire();
            }

            spriteRender.sprite = frames[2];

            if (player.transform.position.x > transform.position.x) faceDir = -1;
            else faceDir = 1;
        }
        else
        {
            animTime += Time.deltaTime;
            if (animTime >= 0.1f)
            {
                animTime = 0f;
                animFrame = 1 - animFrame;

                spriteRender.sprite = frames[animFrame];
            }


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
        if (player.GetComponent<Kaiju>().Dead) return;

        gunLine.enabled = true;
        attackSound.pitch = Random.Range(0.9f, 1.1f);
        attackSound.Play();
        gunLine.SetPosition(0, transform.position);

        Vector3 targetPosition = player.transform.position;

        targetPosition.y = 1.2f+Random.Range(0f,1f);
        targetPosition.x += Random.Range(-0.2f, 0.2f);

        gunLine.SetPosition(1, targetPosition);

        Kaiju kaiju = player.GetComponent<Kaiju>();

        kaiju.Health -= DamagePerShot;
        for (int i = 0; i < 10; i++)
            PlaygroundC.GetParticles(3).Emit(targetPosition+ new Vector3(0f,0f,-0.3f), Random.insideUnitSphere * 0.5f);

    }




    public override void Init()
    {
        spriteRender = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();

        spriteRender.color = new Color(0.1f, 0.2f, 0.5f);

        base.Init();
    }

    protected override void SetUpAudio()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {

            if (source.clip.name.Equals("Shot"))
            {
                attackSound = source;
            }
            else if (source.clip.name.Equals("Human Footstep"))
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
