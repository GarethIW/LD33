using UnityEngine;
using System.Collections;
using System;

public class Citizen : Enemy
{


   
    // Update is called once per frame
    protected override void Update()
    {

        coolDownTimer += Time.deltaTime;
        movementCooldowntimer += Time.deltaTime;

        if (transform.gameObject.activeSelf && GetComponentInChildren<Fire>() != null)
        {

            Health -= FlameDamage;
            Debug.Log("Update() Health" + Health);
        }

        if (transform.gameObject.activeSelf && Health <= 0f)
        {
            GameManager.Instance.score += 5;

            Debug.Log("Update() Score " + GameManager.Instance.score);
            gameObject.SetActive(false);
        }


        if (movementCooldowntimer >= MovementCoolDown)
        {
            currentMovementTarget = getExploringPoint();

            movementCooldowntimer = 0f;
        }

        transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);




    }

    protected override void Fire()
    {

    }
}
