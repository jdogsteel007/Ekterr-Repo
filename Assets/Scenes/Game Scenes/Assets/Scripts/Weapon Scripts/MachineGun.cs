using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BaseWeapon {

	// Use this for initialization
	void Start ()
    {
        _timeSinceLastFire = FireRate;
    }
	
	// Update is called once per frame
	void Update () {
	}

	public override void Fire ()
	{
        CreateBullet();
    }
}
