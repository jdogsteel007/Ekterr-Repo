using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truckScript : MonoBehaviour {

    public float speed = .2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(new Vector3(0, speed, 0));

	}
}
