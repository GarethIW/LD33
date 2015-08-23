using UnityEngine;
using System.Collections;

public class Trooper : Enemy
{

       
    LineRenderer gunLine;
   
   

    public override void Awake()
    {
        base.Awake();
       
        gunLine = GetComponent<LineRenderer>();
       
       
    }



    
    protected override void Update()
    {
        gunLine.enabled = false;
        base.Update();
    }

   

    protected override void Fire()
    {
        gunLine.enabled = true;
        attackSound.Play();
        gunLine.SetPosition(0, transform.position);

        Vector3 targetPosition = player.transform.position;
        targetPosition.y = 1.2f+Random.Range(0f,1f);
        targetPosition.z += 0.15f;

        gunLine.SetPosition(1, targetPosition);
    }

    






}
