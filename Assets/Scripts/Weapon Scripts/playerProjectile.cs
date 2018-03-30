using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectile : MonoBehaviour {

    // Use this for initialization

    public GameObject playerProj;
    Rigidbody2D myRigid;
    public float strength, BulletSpeed = 5f, FireRate = 0.5f;
    private float timeSinceLastFire = 0;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {



        if (Input.GetMouseButton(0) && timeSinceLastFire > FireRate)    //Made it so you can hold down the mouse button for constant fire, just change it back if you didn't want that
        {
            timeSinceLastFire = 0;
            /*
            var go = Instantiate(playerProj, transform.position, Quaternion.identity) as GameObject;
            Physics2D.IgnoreCollision(go.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            myRigid =  go.GetComponent<Rigidbody2D>();

            Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = (Input.mousePosition - sp).normalized;
            myRigid.AddForce(dir * strength);
            */

            Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);

            GameObject bullet = Instantiate(Globals.Inst.DefaultBulletPrefab, transform.position, StaticHelper.LookAt2D(sp, Input.mousePosition));   //new bullet class
            bullet.GetComponent<Bullet>().Creator = gameObject;
            bullet.GetComponent<Bullet>().MovementSpeed = BulletSpeed;
        }

        timeSinceLastFire += Time.deltaTime;

    }
}
