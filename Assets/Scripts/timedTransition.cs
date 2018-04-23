using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timedTransition : MonoBehaviour {

    public float secondsToWait;
    public string sceneToGoTo;

	// Use this for initialization
	void Start () {

        StartCoroutine(delayedStart());

	}
	
	// Update is called once per frame
	void Update () {
		


	}

    IEnumerator delayedStart() {

        yield return new WaitForSeconds(secondsToWait);

        SceneManager.LoadScene(sceneToGoTo);


    }

}
