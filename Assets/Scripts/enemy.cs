using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public GameObject player;
    public bool isChasing = false;
    private bool isColliding = false;

    public int health = 100;


    // Use this for initialization
    void Start () {
		
	}

    IEnumerator detractHealth()
    {
        for (float f = 50f; f >= 0; f -= 1f)
        {
            healthBarScript.health -= 10;
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update () {
        if(isChasing == true)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, .045f);
        }

        if (health == 0) {
            Destroy(gameObject);
        }

        if (isColliding == false)
        {

            StopCoroutine("detractHealth");

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;

        if (isColliding == true && collision.gameObject.tag == "player")
        {

            StartCoroutine("detractHealth");

        }


        if (collision.gameObject.tag == "firstprojectile") {
            health -= 20;
        }
        

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }

}
