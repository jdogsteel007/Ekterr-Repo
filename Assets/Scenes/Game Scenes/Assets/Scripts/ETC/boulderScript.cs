using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class boulderScript : MonoBehaviour {

    public GameObject player;
    public GameObject truck;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

}
