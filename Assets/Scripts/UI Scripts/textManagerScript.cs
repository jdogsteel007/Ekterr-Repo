using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textManagerScript : MonoBehaviour {

    public GameObject canvas;

	// Use this for initialization
	void Start () {

        fadeScript myFadeScript = canvas.GetComponent<fadeScript>();

        myFadeScript.FadeMe();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
