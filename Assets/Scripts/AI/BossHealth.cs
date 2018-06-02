using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossHealth : CombatEntity {

	public GameObject pos1;
	public GameObject pos2;
	public GameObject pos3;

	public float speed;



	//for phase 1&2 shooting
	public int maxShootRange = 100, MaxViewRange = 200, minRange = 5;
	public GameObject enemyProj;
	Rigidbody2D myRigid;
	private float _timeSinceLastFire = 0;

	//for phase 3 chasing player
	public bool isChasing = false;
	private bool isColliding = false;
	public float ChaseSpeed = 5, MaxRange = 40;

	// for phase 3 boar attack
	public Sprite DashWarningSprite;
	public int DashUnits = 5;
	public float DashCooldown = 3, DashSpeed = 1, PreDashTime = 1;
	public bool IsDashingOrAboutToDash = false;

	// Use this for initialization
	private float _timeSinceLastDash = 0;

	//spawmimg enemies
	//public PlayerHealth playerHealth;  //getting the players health
	public GameObject enemy;
	public float spawnTime = 3f;
	public Transform[] spawnPoints; 


	// Use this for initialization
	void Start () {
		Health = MaxHealth; //100 for now, for simplicity of coding/testing
		StartCoroutine(StartWaitDelayCoroutine());
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	// Update is called once per frame
	void Update () {                      //call different functions for each stage of health, so end up with 3 main functions
		
		if (Health > 66){
			//transform.position = Vector3.Lerp (pos1, pos2, (Mathf.Sin (Time.time * speed) + 1.0f) / 2.0f );
			Phase1 ();

		}
		if (Health <= 66 && Health > 33) {
			Phase2 ();
		}
		if (Health <= 33){
			Phase3 ();
		}
	}

	void Phase1 (){
		//Move back And forth
		speed = 1;
		transform.position = Vector3.Lerp (pos1.transform.position, pos2.transform.position, (Mathf.Sin (Time.time * speed) + 1.0f) / 2.0f );  //moce back and forth
		print("we in phase 1");

		//rangedEnemy attack
		Shooting();

		//spawn enemies
		SpawnP1();

	}

	void Phase2 (){
		// no movement
		transform.position = pos3.transform.position;
		print("we in phase 2");

		//rangedEnemy attack
		Shooting();

		//spawn enemies
	}

	void Phase3 (){
		print("we in phase 3");

		//chase enemy
		transform.rotation = StaticHelper.LookAt2D(transform.position, Globals.Inst.Player.transform.position);
		GetComponent<Rigidbody2D>().velocity = transform.up.normalized * ChaseSpeed;

		//if (isChasing == true) {
		//	GetComponent<Rigidbody2D> ().velocity = transform.up.normalized * ChaseSpeed;
		//}

		//boar attack
		if (!IsDashingOrAboutToDash)
			{
		//		isChasing = true; 
				transform.rotation = StaticHelper.LookAt2D(transform.position, Globals.Inst.Player.transform.position);
				_timeSinceLastDash += Time.deltaTime;

				if (_timeSinceLastDash > DashCooldown)
				{
		//			isChasing = false;
					IsDashingOrAboutToDash = true;
					StartCoroutine(Dash());
					_timeSinceLastDash = 0;
				}
			}
		//make him freeze before dashing, dash sprites not showing?


		//spawn enemies

		}

	void Shooting () {
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
		
	public IEnumerator Dash()
	{
		//warning
		List<GameObject> AlertIcons = new List<GameObject>();
		for (int i = 0; i < DashUnits; i++)
		{
			yield return new WaitForSeconds(0.2f);
			GameObject go = new GameObject();
			go.AddComponent<SpriteRenderer>();
			go.GetComponent<SpriteRenderer>().sprite = DashWarningSprite;
			go.transform.position = transform.position + transform.up * (i + 1);
			AlertIcons.Add(go);
		}
		yield return new WaitForSeconds(0.25f);
		foreach (GameObject go in AlertIcons)
			Destroy(go);
		//dashing
		Vector3 target = transform.position + transform.up * DashUnits;
		target.z = 0;
		float timeSinceBeginDash = 0;
		timeSinceBeginDash += Time.deltaTime;
		while ((transform.position - target).magnitude > 0.01)
		{
			Vector3 newPos = Vector3.Lerp(transform.position, target, 0.5f);
			RaycastHit2D rhh = Physics2D.Raycast(transform.position + transform.up, transform.forward, DashUnits); //will break if boar is scaled, for now
			if (!rhh)
			{
				transform.position = newPos;
			}
			else
			{
				transform.position = new Vector3(rhh.point.x, rhh.point.y, 0) - transform.up;
				Debug.Log("Boar Hit: ");
				break;
			}
			yield return null;
		}
		IsDashingOrAboutToDash = false;
		yield return null;
	}

	void SpawnP1(){
		print("how tf we do this");
		//if player has no health left
		//if (playerHealth <= 0f) {
			// ... exit the function
	//		return;
	//	}

		//need a timer for enemies spawning
		spawnTime -= Time.deltaTime;
		print (spawnTime);
		if (spawnTime == 0) {
			//find a random index between zero and one less than the number of spawn points
			int spawnPointsIndex = Random.Range (0, spawnPoints.Length);

			//create an instance of the enemy prefab at the randomly selected spawn points position
			Instantiate (enemy, spawnPoints [spawnPointsIndex].position, spawnPoints [spawnPointsIndex].rotation);
			spawnTime = 3f;
		}
	}


	}


