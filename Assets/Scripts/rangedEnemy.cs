using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemy : MonoBehaviour {

    public GameObject player;
    public GameObject rangeEnemy;
    public bool isChasing = false;

    public int health = 100;

    public int maxRange;
    public int minRange;

    public GameObject enemyProj;
    Rigidbody2D myRigid;
    public float strength;

    Vector3 addPos = new Vector3(2f, 2f);

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if (isChasing == true)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, .045f);
        }

        if ((Vector3.Distance(transform.position, player.transform.position) < maxRange)
            && (Vector3.Distance(transform.position, player.transform.position) > minRange))
        {
            isChasing = false;

            rangeEnemy.transform.LookAt(player.transform);

            var go = Instantiate(enemyProj, transform.position, Quaternion.identity) as GameObject;
            Physics2D.IgnoreCollision(go.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            myRigid = go.GetComponent<Rigidbody2D>();

            
            Vector3 dir = player.transform.position;
            myRigid.AddForce(dir * strength);

        }

        if (health == 0)
        {
            Destroy(gameObject);
        }

    }
}
