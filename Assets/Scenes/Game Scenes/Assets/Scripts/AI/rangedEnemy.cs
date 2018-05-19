using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemy : CombatEntity {

    //public GameObject player; //(Devin) Access this from Globals.Inst now
    //public GameObject rangeEnemy; //(Devin) Don't actually need this, because you can get the attached gameobject from the property inhereted from MonoBehaviour
    public bool isChasing = false;

    public int maxShootRange = 50, MaxViewRange = 75, minRange = 5; //Health is now in CombatEntity

    public GameObject enemyProj;
    Rigidbody2D myRigid;
    public float Strength, ChaseSpeed = 0.45f, FireRate = 1f, BulletSpeed = 10f;

    //private Vector3 addPos = new Vector3(2f, 2f); //??
    private float _timeSinceLastFire = 0;

    // Use this for initialization
    void Start () {
        StartCoroutine(StartWaitDelayCoroutine());
        Health = MaxHealth;
        IsFriendly = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (IsActiveInGame && Vector3.Distance(transform.position, Globals.Inst.Player.transform.position) < MaxViewRange)
        {

            transform.rotation = StaticHelper.LookAt2D(transform.position, Globals.Inst.Player.transform.position);

            if (
                (!UseRaycastVision || (UseRaycastVision && RaycastPlayer())) &&
                Vector3.Distance(transform.position, Globals.Inst.Player.transform.position) < maxShootRange && 
                Vector3.Distance(transform.position, Globals.Inst.Player.transform.position) > minRange)
            {
                if (GetComponent<Rigidbody2D>().velocity.magnitude > 0) GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isChasing = false;
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

}
