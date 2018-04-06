using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemy : CombatEntity {

    //public GameObject player; //(Devin) Access this from Globals.Inst now
    //public GameObject rangeEnemy; //(Devin) Don't actually need this, because you can get the attached gameobject from the property inhereted from MonoBehaviour
    public bool isChasing = false;

    public int maxRange = 50, minRange = 5; //Health is now in CombatEntity

    public GameObject enemyProj;
    Rigidbody2D myRigid;
    public float Strength, ChaseSpeed = 0.45f, FireRate = 1f, BulletSpeed = 10f;

    //private Vector3 addPos = new Vector3(2f, 2f); //??
    private float _timeSinceLastFire = 0;

    // Use this for initialization
    void Start () {
        Health = MaxHealth;
        IsFriendly = false;
	}
	
	// Update is called once per frame
	void Update () {

        transform.rotation = StaticHelper.LookAt2D(transform.position, Globals.Inst.Player.transform.position);

        if (Vector3.Distance(transform.position, Globals.Inst.Player.transform.position) < maxRange && Vector3.Distance(transform.position, Globals.Inst.Player.transform.position) > minRange)
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude > 0) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isChasing = false;

            /*
            var go = Instantiate(enemyProj, transform.position, Quaternion.identity) as GameObject;
            Physics2D.IgnoreCollision(go.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            myRigid = go.GetComponent<Rigidbody2D>();
            */
            //Vector3 dir = player.transform.position;
            //myRigid.AddForce(dir * Strength);

            /*
            if (_timeSinceLastFire > FireRate)
            {
                //fire bullet and reset time counter
                GameObject bullet = Instantiate(Globals.Inst.DefaultBulletPrefab, transform.position + transform.up, transform.rotation);
                bullet.GetComponent<Bullet>().MovementSpeed = BulletSpeed;
                bullet.GetComponent<Bullet>().Creator = gameObject;
                bullet.GetComponent<Bullet>().FriendlyBullet = IsFriendly;
                bullet.GetComponent<SpriteRenderer>().color = Color.red;
                _timeSinceLastFire = 0;
            }
            else
                _timeSinceLastFire += Time.deltaTime;
                */
            if (GetComponent<BaseWeapon>())
            {
                GetComponent<BaseWeapon>().TryToFire();
            }

        }
        else
        {
            isChasing = true;
            GetComponent<Rigidbody2D>().velocity = transform.up.normalized * ChaseSpeed;
        }

        /*
        if (Health == 0)
        {
            Destroy(gameObject);
        }
        */

    }
}
