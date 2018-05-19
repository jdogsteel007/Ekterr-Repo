using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSwitch : MonoBehaviour {

    public string sceneToSwitchTo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.name == "player")
        {
            Debug.Log("levelswitch collision");
            SceneManager.LoadScene(sceneToSwitchTo);
        }

    }

}
