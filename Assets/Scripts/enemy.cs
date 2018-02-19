using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public GameObject player;
    public bool isChasing = false;

    public int health = 100;

	// Use this for initialization
	void Start () {
		
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

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "player"){
            healthBarScript.health -= 10f;
            Debug.Log("hit");
        }

        if (collision.gameObject.tag == "firstprojectile") {
            health -= 20;
        }
        

        
    }

}
