using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour {

    // Use this for initialization

    public GameObject playerProj;
    Rigidbody2D myRigid;
    public float strength;
    private float fireRate = 1f;
    private float timeSinceLastFire = 0;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



        if (Input.GetMouseButtonDown(0) && timeSinceLastFire > fireRate)
        {
            timeSinceLastFire = 0;
            var go = Instantiate(playerProj, transform.position, Quaternion.identity) as GameObject;
            Physics2D.IgnoreCollision(go.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            myRigid =  go.GetComponent<Rigidbody2D>();

            Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = (Input.mousePosition - sp).normalized;
            myRigid.AddForce(dir * strength);
        }

        timeSinceLastFire += Time.deltaTime;

    }
}
