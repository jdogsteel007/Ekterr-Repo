﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : CombatEntity {

    //public GameObject player; //Get this from Globals.Inst now
    public bool isChasing = false;
    private bool isColliding = false;
    public float ChaseSpeed = 5, MaxRange = 20;


    // Use this for initialization
    void Start () {
        StartCoroutine(StartWaitDelayCoroutine());
        IsFriendly = false;
        Health = MaxHealth;
	}

    IEnumerator detractHealth()
    {
        for (float f = 50f; f >= 0; f -= 1f)
        {
            //healthBarScript.health -= 10;
            Globals.Inst.Player.Health -= 2;
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update () {

        if (isChasing == true && IsActiveInGame && Vector3.Distance(transform.position, Globals.Inst.transform.position) < MaxRange)
        {
            if (!UseRaycastVision || (UseRaycastVision && RaycastPlayer()))
            {
                transform.rotation = StaticHelper.LookAt2D(transform.position, Globals.Inst.Player.transform.position);
                GetComponent<Rigidbody2D>().velocity = transform.up.normalized * ChaseSpeed;
            } //else the enemy doesn't see the player
            else GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (Health == 0) {
            Destroy(gameObject);
        }

        if (!isColliding)
        {

            StopCoroutine("detractHealth");

        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;

        if (isColliding && collision.gameObject.tag == "player")
        {

            StartCoroutine("detractHealth");

        }


        if (collision.gameObject.tag == "firstprojectile") {
            Health -= 20;
        }
        

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }

}
