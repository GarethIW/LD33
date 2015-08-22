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



    // Use this for initialization
    void Start()
    {



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
        gunLine.SetPosition(1, player.transform.position);
    }

    






}
