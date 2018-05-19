using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject followTarget;
    //because we dont want the z value of the camera to be 0 - that would make the game unseeable
    private Vector3 targetPosition;
    public float cameramoveSpeed;

	// Use this for initialization
	void Start () {
        Globals.Inst.MainCamera = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //the last parameter is the original z value of the camera
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        //first parameter is where we are, second is where we want to go, third is the speed at which we want to traverse multiplied by time.deltatime to make movement independent of framerate
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameramoveSpeed * Time.deltaTime);

    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
