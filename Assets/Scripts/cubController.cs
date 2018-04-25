using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubController : MonoBehaviour {

    public GameObject player;
    Vector3 buffer = new Vector3(-.1f, 2f, 0f);

	// Use this for initialization
	void Start () {
		
	}

    public float moveSpeed;
    // Update is called once per frame
    void Update () {

        if (Globals.Inst.InputFocus == gameObject)
        {
            if (Globals.DidPlayerSwitchThisFrame)
                Globals.DidPlayerSwitchThisFrame = false;
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Globals.DidPlayerSwitchThisFrame = true;
                Globals.Inst.InputFocus = Globals.Inst.Player.gameObject;
                Globals.Inst.MainCamera.GetComponent<CameraController>().followTarget = Globals.Inst.Player.gameObject;
                return;
            }
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));   //(Devin) here is a cleaner way of doing player movement, I hope you don't mind...
            if (movement.magnitude > 0.1)
            {
                GetComponent<Rigidbody2D>().velocity = movement.normalized * moveSpeed;
            }
            else GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else //player is not controlling cub
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.position = Vector3.Lerp(transform.position, player.transform.position - buffer, .03f);
        }
    }
}
