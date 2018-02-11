using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubController : MonoBehaviour {

    public GameObject player;
    Vector3 buffer = new Vector3(-.1f, 2f, 0f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector3.Lerp(transform.position, player.transform.position - buffer, .04f);

    }
}
