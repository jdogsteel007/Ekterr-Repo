using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class truckScript : MonoBehaviour {

    public float speed = 0;
    public float secondsToWait = 3;
    public Canvas runCanvas;
    public GameObject player;
    private Rigidbody2D playerRigid;

	// Use this for initialization
	void Start () {
        StartCoroutine(levelPause());
        playerRigid = player.GetComponent<Rigidbody2D>();
        playerRigid.constraints = RigidbodyConstraints2D.FreezeAll;
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

        if (collision.gameObject.tag == "player") {
            Destroy(collision.gameObject);
            SceneManager.LoadScene("gameover");
        }
    }

    IEnumerator levelPause() {
        //truck pauses at beginning so words can be read about running away

        yield return new WaitForSeconds(secondsToWait);
        speed = .114f;
        runCanvas.enabled = false;
        playerRigid.constraints = RigidbodyConstraints2D.None;
        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

}
