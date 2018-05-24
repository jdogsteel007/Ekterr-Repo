using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossHealth : CombatEntity {


	public GameObject pos1;
	public GameObject pos2;
	public GameObject pos3;

	public float speed;


	// Use this for initialization
	void Start () {
		Health = MaxHealth; //100 for now, for simplicity of coding/testing

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
		speed = 1;
		transform.position = Vector3.Lerp (pos1.transform.position, pos2.transform.position, (Mathf.Sin (Time.time * speed) + 1.0f) / 2.0f );  //moce back and forth

		//rangedEnemy attack
		//Move back And forth
		//spawn enemies
	}

	void Phase2 (){
		// no movement
		transform.position = pos3.transform.position;

		//rangedEnemy attack

		//spawn enemies
	}

	void Phase3 (){
		//boar attack
		//spawn enemies
	}

}
