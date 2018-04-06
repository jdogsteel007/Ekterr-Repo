using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BaseWeapon {

    public int BulletSpread = 20, NumberOfBullets = 10;

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
		for (int i = 0; i < NumberOfBullets; i++) {
            GameObject bullet = CreateBullet();
			bullet.transform.rotation = Quaternion.Euler(bullet.transform.rotation.eulerAngles + new Vector3 (0, 0, Random.Range(-BulletSpread, BulletSpread)));
		}
        base.Fire();
	}
}
