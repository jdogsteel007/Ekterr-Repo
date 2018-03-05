using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textManagerScript : MonoBehaviour {

    public GameObject canvas;
    public float waitFor = 0;
    public float waitForDissolve;

	// Use this for initialization
	void Start () {


        StartCoroutine("myDelay");

	}
	
	// Update is called once per frame
	void Update () {
        fadeScript myFadeScript = canvas.GetComponent<fadeScript>();

        if (myFadeScript.alpha >= 1)
        {
            StopCoroutine("myDelay");
            StartCoroutine("secondDelay");
        }
    }

    IEnumerator myDelay() {


        yield return new WaitForSeconds(waitFor);

        fadeScript myFadeScript = canvas.GetComponent<fadeScript>();
        myFadeScript.FadeMe();

    }

    IEnumerator secondDelay()
    {


        yield return new WaitForSeconds(waitForDissolve);

        fadeScript myFadeScript = canvas.GetComponent<fadeScript>();
        myFadeScript.undoFadeMe();

    }



}
