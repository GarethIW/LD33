using UnityEngine;
using System.Collections;

public class Citizen : Enemy
{


   
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

        if (!isOnFire && Vector3.Distance(player.transform.position, transform.position) < 5f)
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

            if (Vector3.Distance(player.transform.position, transform.position) > 10f)
                isFleeing = false;
        }


        // transform.position += getMoveTowardsVector(transform.position, currentMovementTarget);


        base.Update();

    }

    protected override void Fire()
    {

    }
}
