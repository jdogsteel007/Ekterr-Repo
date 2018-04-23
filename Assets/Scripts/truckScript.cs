using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truckScript : MonoBehaviour {

    public float speed = 0;
    private float secondsToWait = 3;

	// Use this for initialization
	void Start () {
        StartCoroutine(levelPause());
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(new Vector3(0, -speed, 0));

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "truck") {
            speed = 0;
        }
    }

    IEnumerator levelPause() {
        //truck pauses at beginning so words can be read about running away

        yield return new WaitForSeconds(secondsToWait);
        speed = .11f;

    }

}
